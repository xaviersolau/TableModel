// ----------------------------------------------------------------------
// <copyright file="TableModelController.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using SoloX.TableModel.Dto;
using System;
using System.Threading.Tasks;
using SoloX.TableModel.Impl;
using SoloX.TableModel.Services;
using SoloX.ExpressionTools.Parser.Impl;
using System.Collections.Generic;
using SoloX.ExpressionTools.Parser.Impl.Resolver;

namespace SoloX.TableModel.Server
{
    /// <summary>
    /// Table model controller implementation.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TableModelController : ControllerBase
    {
        private readonly ITableDataRepository tableDataRepository;
        private readonly IDtoToTableModelService dtoToTableModelService;

        /// <summary>
        /// Setup the table model controller.
        /// </summary>
        /// <param name="tableDataRepository">The table data repository.</param>
        /// <param name="dtoToTableModelService">The dto to table model service.</param>
        public TableModelController(ITableDataRepository tableDataRepository, IDtoToTableModelService dtoToTableModelService)
        {
            this.tableDataRepository = tableDataRepository;
            this.dtoToTableModelService = dtoToTableModelService;
        }

        /// <summary>
        /// Post a data request on the table data matching the given table data Id.
        /// </summary>
        /// <param name="id">The table data Id to request.</param>
        /// <param name="request">The data request to run.</param>
        /// <returns>The requested data.</returns>
        [HttpPost("{id}")]
        public async Task<IActionResult> PostDataRequestAsync(string id, [FromBody] DataRequestDto request)
        {
            var tableData = await this.tableDataRepository.GetTableDataAsync(id).ConfigureAwait(false);

            return await tableData.Accept(new Visitor(this.dtoToTableModelService), request).ConfigureAwait(false);
        }

        /// <summary>
        /// Post a data count request on the table data matching the given table data Id.
        /// </summary>
        /// <param name="id">The table data Id to request.</param>
        /// <param name="request">The data count request to run.</param>
        /// <returns>The requested data count.</returns>
        [HttpPost("{id}/Count")]
        public async Task<IActionResult> PostDataCountRequestAsync(string id, [FromBody] DataCountRequestDto request)
        {
            var tableData = await this.tableDataRepository.GetTableDataAsync(id).ConfigureAwait(false);

            return await tableData.Accept(new CountVisitor(this.dtoToTableModelService), request).ConfigureAwait(false);
        }

        private class Visitor : ITableDataVisitor<Task<IActionResult>, DataRequestDto>
        {
            private readonly IDtoToTableModelService dtoToTableModelService;

            public Visitor(IDtoToTableModelService dtoToTableModelService)
            {
                this.dtoToTableModelService = dtoToTableModelService;
            }

            public async Task<IActionResult> Visit<TData>(ITableData<TData> tableData, DataRequestDto request)
            {
                var offset = request.Offset;
                var pageSize = request.PageSize;

                ITableFilter<TData> filter = new TableFilter<TData>();
                if (request.Filters != null)
                {
                    var columnVisitor = new ColumnVisitor<TData>(filter);

                    foreach (var requestFilter in request.Filters)
                    {
                        var column = this.dtoToTableModelService.Map<TData>(requestFilter.Column);

                        column.Accept(columnVisitor, requestFilter.FilterExpression);
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

                return new OkObjectResult(data);
            }
        }

        private class CountVisitor : ITableDataVisitor<Task<IActionResult>, DataCountRequestDto>
        {
            private readonly IDtoToTableModelService dtoToTableModelService;

            public CountVisitor(IDtoToTableModelService dtoToTableModelService)
            {
                this.dtoToTableModelService = dtoToTableModelService;
            }

            public async Task<IActionResult> Visit<TData>(ITableData<TData> tableData, DataCountRequestDto request)
            {
                ITableFilter<TData> filter = new TableFilter<TData>();
                if (request.Filters != null)
                {
                    var columnVisitor = new ColumnVisitor<TData>(filter);

                    foreach (var requestFilter in request.Filters)
                    {
                        var column = this.dtoToTableModelService.Map<TData>(requestFilter.Column);

                        column.Accept(columnVisitor, requestFilter.FilterExpression);
                    }
                }

                var dataCount = await tableData.GetDataCountAsync(filter).ConfigureAwait(false);

                return new OkObjectResult(dataCount);
            }
        }

        private class ColumnVisitor<TData> : IColumnVisitor<TData, object, string>
        {
            private readonly ITableFilter<TData> tableFilter;

            public ColumnVisitor(ITableFilter<TData> tableFilter)
            {
                this.tableFilter = tableFilter;
            }

            public object Visit<TColumn>(IColumn<TData, TColumn> column, string filter)
            {
                var expressionParser = new ExpressionParser(new SingleParameterTypeResolver(typeof(TColumn)), new StaticMethodResolver(typeof(string)));

                var filterExpression = expressionParser.Parse<Func<TColumn, bool>>(filter);

                this.tableFilter.Register(column, filterExpression);

                return null;
            }
        }
    }
}