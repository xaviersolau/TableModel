using Microsoft.AspNetCore.Mvc;
using SoloX.TableModel.Dto;
using System;
using System.Threading.Tasks;
using SoloX.TableModel.Impl;
using SoloX.TableModel.Services;
using SoloX.ExpressionTools.Parser.Impl;
using SoloX.TableModel.Services.Impl;
using System.Collections.Generic;
using SoloX.ExpressionTools.Parser.Impl.Resolver;

namespace SoloX.TableModel.Server
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableModelController : ControllerBase
    {
        private readonly ITableDataRepository tableDataRepository;
        private readonly IDtoToTableModelService dtoToTableModelService;

        public TableModelController(ITableDataRepository tableDataRepository, IDtoToTableModelService dtoToTableModelService)
        {
            this.tableDataRepository = tableDataRepository;
            this.dtoToTableModelService = dtoToTableModelService;
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> PostDataRequestAsync(string id, [FromBody] DataRequestDto request)
        {
            var tableData = await tableDataRepository.GetTableDataAsync(id);

            return await tableData.Accept(new Visitor(this.dtoToTableModelService), request);
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
                    data = await tableData.GetDataPageAsync(sorting, filter, offset.Value, pageSize.Value);
                }
                else if (offset == null && pageSize == null)
                {
                    data = await tableData.GetDataAsync(sorting, filter);
                }
                else
                {
                    throw new ArgumentException($"Bad request: {nameof(request.Offset)} {nameof(request.PageSize)} must be both set or both null");
                }

                return new OkObjectResult(data);
            }
        }

        private class ColumnVisitor<TData> : IColumnVisitor<TData, object, string>
        {
            private ITableFilter<TData> tableFilter;

            public ColumnVisitor(ITableFilter<TData> tableFilter)
            {
                this.tableFilter = tableFilter;
            }

            public object Visit<TColumn>(IColumn<TData, TColumn> column, string filter)
            {
                var expressionParser = new ExpressionParser(new SingleParameterTypeResolver(typeof(TColumn)));

                var filterExpression = expressionParser.Parse<Func<TColumn, bool>>(filter);

                tableFilter.Register(column, filterExpression);

                return null;
            }
        }
    }
}