// ----------------------------------------------------------------------
// <copyright file="TableDataEndPointService.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using SoloX.ExpressionTools.Parser.Impl;
using SoloX.ExpressionTools.Parser.Impl.Resolver;
using SoloX.TableModel.Dto;
using SoloX.TableModel.Impl;
using SoloX.TableModel.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.TableModel.Server.Services.Impl
{
    /// <summary>
    /// TableData End-Point service implementation.
    /// </summary>
    public class TableDataEndPointService : ITableDataEndPointService
    {
        private readonly ITableDataRepository tableDataRepository;
        private readonly IDtoToTableModelService dtoToTableModelService;
        private readonly ILogger<TableDataEndPointService> logger;

        /// <summary>
        /// Setup TableDataEndPointService.
        /// </summary>
        /// <param name="tableDataRepository">The table data repository.</param>
        /// <param name="dtoToTableModelService">The dto to table model service.</param>
        /// <param name="logger">Logger where to log request information.</param>
        public TableDataEndPointService(ITableDataRepository tableDataRepository, IDtoToTableModelService dtoToTableModelService, ILogger<TableDataEndPointService> logger)
        {
            this.tableDataRepository = tableDataRepository;
            this.dtoToTableModelService = dtoToTableModelService;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TableDataDto>> GetRegisteredTableDataAsync()
        {
            var idsAndTypes = await this.tableDataRepository.GetTableDataIdsAndTypesAsync().ConfigureAwait(false);

            return idsAndTypes.Select(i => new TableDataDto()
            {
                Id = i.Id,
                DataType = i.DataType.AssemblyQualifiedName,
            });
        }

        /// <inheritdoc/>
        public async Task<int> ProcessDataCountRequestAsync(string id, DataCountRequestDto request)
        {
            var tableData = await this.tableDataRepository.GetTableDataAsync(id).ConfigureAwait(false);

            return await tableData.Accept(new CountVisitor(this.dtoToTableModelService, this.logger), request).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TData>> ProcessDataRequestAsync<TData>(string id, DataRequestDto request)
        {
            var tableData = await this.tableDataRepository.GetTableDataAsync(id).ConfigureAwait(false);

            var data = await tableData.Accept(new DataVisitor(this.dtoToTableModelService, this.logger), request).ConfigureAwait(false);

            return (IEnumerable<TData>)data;
        }

        private class DataVisitor : ITableDataVisitor<Task<IEnumerable>, DataRequestDto>
        {
            private readonly IDtoToTableModelService dtoToTableModelService;
            private readonly ILogger<TableDataEndPointService> logger;

            public DataVisitor(IDtoToTableModelService dtoToTableModelService, ILogger<TableDataEndPointService> logger)
            {
                this.dtoToTableModelService = dtoToTableModelService;
                this.logger = logger;
            }

            public async Task<IEnumerable> Visit<TData>(ITableData<TData> tableData, DataRequestDto request)
            {
                var offset = request.Offset;
                var pageSize = request.PageSize;

                ITableFilter<TData> filter = new TableFilter<TData>();
                if (request.Filters != null)
                {
                    var columnVisitor = new ColumnVisitor<TData>(filter, this.logger);

                    foreach (var requestFilter in request.Filters)
                    {
                        if (requestFilter.Column != null)
                        {
                            var column = this.dtoToTableModelService.Map<TData>(requestFilter.Column);

                            column.Accept(columnVisitor, requestFilter.FilterExpression);
                        }
                        else
                        {
                            var expressionParser = new ExpressionParser(new SingleParameterTypeResolver(typeof(TData)), new StaticMethodResolver(typeof(string)));

                            this.logger.LogDebug($"Parsing filter: " + requestFilter.FilterExpression);

                            var filterExpression = expressionParser.Parse<Func<TData, bool>>(requestFilter.FilterExpression);

                            filter.Register(filterExpression);
                        }
                    }
                }

                ITableSorting<TData> sorting = new TableSorting<TData>();
                if (request.Sortings != null)
                {
                    foreach (var requestSorting in request.Sortings)
                    {
                        var column = this.dtoToTableModelService.Map<TData>(requestSorting.Column);

                        sorting.Register(column, requestSorting.Order);
                    }
                }

                IEnumerable<TData> data;

                if (offset != null && pageSize != null)
                {
                    data = await tableData.GetDataPageAsync(sorting, filter, offset.Value, pageSize.Value).ConfigureAwait(false);
                }
                else if (offset == null && pageSize == null)
                {
                    data = await tableData.GetDataAsync(sorting, filter).ConfigureAwait(false);
                }
                else
                {
                    throw new ArgumentException($"Bad request: {nameof(request.Offset)} {nameof(request.PageSize)} must be both set or both null");
                }

                return data;
            }
        }

        private class CountVisitor : ITableDataVisitor<Task<int>, DataCountRequestDto>
        {
            private readonly IDtoToTableModelService dtoToTableModelService;
            private readonly ILogger<TableDataEndPointService> logger;

            public CountVisitor(IDtoToTableModelService dtoToTableModelService, ILogger<TableDataEndPointService> logger)
            {
                this.dtoToTableModelService = dtoToTableModelService;
                this.logger = logger;
            }

            public async Task<int> Visit<TData>(ITableData<TData> tableData, DataCountRequestDto request)
            {
                ITableFilter<TData> filter = new TableFilter<TData>();
                if (request.Filters != null)
                {
                    var columnVisitor = new ColumnVisitor<TData>(filter, this.logger);

                    foreach (var requestFilter in request.Filters)
                    {
                        if (requestFilter.Column != null)
                        {
                            var column = this.dtoToTableModelService.Map<TData>(requestFilter.Column);

                            column.Accept(columnVisitor, requestFilter.FilterExpression);
                        }
                        else
                        {
                            var expressionParser = new ExpressionParser(new SingleParameterTypeResolver(typeof(TData)), new StaticMethodResolver(typeof(string)));

                            this.logger.LogDebug($"Parsing filter: " + requestFilter.FilterExpression);

                            var filterExpression = expressionParser.Parse<Func<TData, bool>>(requestFilter.FilterExpression);

                            filter.Register(filterExpression);
                        }
                    }
                }

                var dataCount = await tableData.GetDataCountAsync(filter).ConfigureAwait(false);

                return dataCount;
            }
        }

        private class ColumnVisitor<TData> : IColumnVisitor<TData, object, string>
        {
            private readonly ITableFilter<TData> tableFilter;
            private readonly ILogger<TableDataEndPointService> logger;

            public ColumnVisitor(ITableFilter<TData> tableFilter, ILogger<TableDataEndPointService> logger)
            {
                this.tableFilter = tableFilter;
                this.logger = logger;
            }

            public object Visit<TColumn>(IColumn<TData, TColumn> column, string filter)
            {
                var expressionParser = new ExpressionParser(new SingleParameterTypeResolver(typeof(TColumn)), new StaticMethodResolver(typeof(string)));

                this.logger.LogDebug($"Parsing filter: " + filter);

                var filterExpression = expressionParser.Parse<Func<TColumn, bool>>(filter);

                this.tableFilter.Register(column, filterExpression);

                return null;
            }
        }
    }
}
