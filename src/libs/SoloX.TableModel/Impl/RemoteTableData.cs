using SoloX.TableModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    public class RemoteTableData<TData> : ATableData<TData>
    {
        private readonly HttpClient httpClient;

        public RemoteTableData(string id, HttpClient httpClient)
            : base(id)
        {
            this.httpClient = httpClient;
        }

        public override Task<IEnumerable<TData>> GetDataPageAsync(int offset, int pageSize)
        {
            var request = MakeDataRequestDto(null, null, offset, pageSize);

            return SendDataRequest(request);
        }

        public override Task<IEnumerable<TData>> GetDataAsync()
        {
            var request = MakeDataRequestDto(null, null, null, null);

            return SendDataRequest(request);
        }

        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter, int offset, int pageSize)
        {
            var request = MakeDataRequestDto(sorting, filter, offset, pageSize);

            return SendDataRequest(request);
        }

        public override Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter)
        {
            var request = MakeDataRequestDto(sorting, filter, null, null);

            return SendDataRequest(request);
        }

        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableFilter<TData> filter, int offset, int pageSize)
        {
            var request = MakeDataRequestDto(null, filter, offset, pageSize);

            return SendDataRequest(request);
        }

        public override Task<IEnumerable<TData>> GetDataAsync(ITableFilter<TData> filter)
        {
            var request = MakeDataRequestDto(null, filter, null, null);

            return SendDataRequest(request);
        }

        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, int offset, int pageSize)
        {
            var request = MakeDataRequestDto(sorting, null, offset, pageSize);

            return SendDataRequest(request);
        }

        public override Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting)
        {
            var request = MakeDataRequestDto(sorting, null, null, null);

            return SendDataRequest(request);
        }

        public override Task<int> GetDataCountAsync()
        {
            var request = MakeDataCountRequestDto(null);

            return SendDataCountRequest(request);
        }

        public override Task<int> GetDataCountAsync(ITableFilter<TData> filter)
        {
            var request = MakeDataCountRequestDto(filter);

            return SendDataCountRequest(request);
        }

        private async Task<IEnumerable<TData>> SendDataRequest(DataRequestDto request)
        {
            var response = await this.httpClient.PostAsJsonAsync(Id, request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<TData>>();
        }

        private async Task<int> SendDataCountRequest(DataCountRequestDto request)
        {
            var response = await this.httpClient.PostAsJsonAsync($"{Id}/Count", request);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<int>();
        }

        private static DataRequestDto MakeDataRequestDto(ITableSorting<TData> sorting, ITableFilter<TData> filter, int? offset, int? pageSize)
        {
            var filterVisitor = new FilterVisitor();
            var columnVisitor = new ColumnVisitor();

            var filters = filter?.ColumnFilters.Select(f => f.Accept(filterVisitor));

            var sortings = sorting?.ColumnSortings.Select(s => new SortingDto()
            {
                Column = s.Column.Accept(columnVisitor),
                Order = s.Order,
            });

            return new DataRequestDto()
            {
                Offset = offset,
                PageSize = pageSize,
                Filters = filters,
                Sortings = sortings,
            };
        }

        private static DataCountRequestDto MakeDataCountRequestDto(ITableFilter<TData> filter)
        {
            var filterVisitor = new FilterVisitor();

            var filters = filter?.ColumnFilters.Select(f => f.Accept(filterVisitor));

            return new DataCountRequestDto()
            {
                Filters = filters,
            };
        }

        private class ColumnVisitor : IColumnVisitor<TData, ColumnDto>
        {
            public ColumnDto Visit<TColumn>(IColumn<TData, TColumn> column)
            {
                return new ColumnDto()
                {
                    Id = column.Id,
                    CanSort = column.CanSort,
                    CanFilter = column.CanFilter,
                    DataType = column.DataType.AssemblyQualifiedName,
                    DataGetterExpression = column.DataGetterExpression.ToString(),
                };
            }
        }

        private class FilterVisitor : IColumnFilterVisitor<TData, FilterDto>
        {
            public FilterDto Visit<TColumn>(IColumnFilter<TData, TColumn> columnFilter)
            {
                var column = columnFilter.Column;

                return new FilterDto()
                {
                    Column = new ColumnDto()
                    {
                        Id = column.Id,
                        CanSort = column.CanSort,
                        CanFilter = column.CanFilter,
                        DataType = column.DataType.AssemblyQualifiedName,
                        DataGetterExpression = column.DataGetterExpression.ToString(),
                    },
                    FilterExpression = columnFilter.Filter.ToString(),
                };
            }
        }
    }
}
