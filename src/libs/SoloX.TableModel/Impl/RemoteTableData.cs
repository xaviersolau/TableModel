// ----------------------------------------------------------------------
// <copyright file="RemoteTableData.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.ExpressionTools.Transform;
using SoloX.TableModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// HTTP remote table data provider.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public class RemoteTableData<TData> : ATableData<TData>
    {
        private readonly HttpClient httpClient;
        private readonly string httpDataSuffix;
        private readonly string httpCountSuffix;

        /// <summary>
        /// Setup a RemoteTableData with the given HttpClient.
        /// </summary>
        /// <param name="id">Table data Id.</param>
        /// <param name="httpClient">HttpClient to use to get the remote data from.</param>
        /// <param name="httpDataSuffix">Url Suffix to use in Http data requests.</param>
        /// <param name="httpCountSuffix">Url Suffix to use in Http count requests.</param>
        public RemoteTableData(string id, HttpClient httpClient, string httpDataSuffix, string httpCountSuffix)
            : base(id)
        {
            if (string.IsNullOrEmpty(httpDataSuffix))
            {
                throw new ArgumentNullException(nameof(httpDataSuffix));
            }

            if (string.IsNullOrEmpty(httpCountSuffix))
            {
                throw new ArgumentNullException(nameof(httpCountSuffix));
            }

            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.httpDataSuffix = httpDataSuffix;
            this.httpCountSuffix = httpCountSuffix;
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataPageAsync(int offset, int pageSize)
        {
            var request = MakeDataRequestDto(null, null, offset, pageSize);

            return SendDataRequestAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataAsync()
        {
            var request = MakeDataRequestDto(null, null, null, null);

            return SendDataRequestAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter, int offset, int pageSize)
        {
            var request = MakeDataRequestDto(sorting, filter, offset, pageSize);

            return SendDataRequestAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter)
        {
            var request = MakeDataRequestDto(sorting, filter, null, null);

            return SendDataRequestAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableFilter<TData> filter, int offset, int pageSize)
        {
            var request = MakeDataRequestDto(null, filter, offset, pageSize);

            return SendDataRequestAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataAsync(ITableFilter<TData> filter)
        {
            var request = MakeDataRequestDto(null, filter, null, null);

            return SendDataRequestAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, int offset, int pageSize)
        {
            var request = MakeDataRequestDto(sorting, null, offset, pageSize);

            return SendDataRequestAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting)
        {
            var request = MakeDataRequestDto(sorting, null, null, null);

            return SendDataRequestAsync(request);
        }

        ///<inheritdoc/>
        public override Task<int> GetDataCountAsync()
        {
            var request = MakeDataCountRequestDto(null);

            return SendDataCountRequestAsync(request);
        }

        ///<inheritdoc/>
        public override Task<int> GetDataCountAsync(ITableFilter<TData> filter)
        {
            var request = MakeDataCountRequestDto(filter);

            return SendDataCountRequestAsync(request);
        }

        private async Task<IEnumerable<TData>> SendDataRequestAsync(DataRequestDto request)
        {
            var response = await this.httpClient.PostAsJsonAsync(GetDataRequestUri(), request).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<IEnumerable<TData>>().ConfigureAwait(false);

            return data ?? Array.Empty<TData>();
        }

        private string GetDataRequestUri()
        {
            return $"{Id}/{this.httpDataSuffix}";
        }

        private async Task<int> SendDataCountRequestAsync(DataCountRequestDto request)
        {
            var response = await this.httpClient.PostAsJsonAsync(GetCountRequestUri(), request).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<int>().ConfigureAwait(false);
        }

        private string GetCountRequestUri()
        {
            return $"{Id}/{this.httpCountSuffix}";
        }

        private static DataRequestDto MakeDataRequestDto(ITableSorting<TData>? sorting, ITableFilter<TData>? filter, int? offset, int? pageSize)
        {
            var filters = MakeFiltersDto(filter);

            var columnVisitor = new ColumnVisitor();
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
            var filters = MakeFiltersDto(filter);

            return new DataCountRequestDto()
            {
                Filters = filters,
            };
        }

        private static IEnumerable<FilterDto>? MakeFiltersDto(ITableFilter<TData>? filter)
        {
            var filterVisitor = new FilterVisitor();
            var filters = filter?.ColumnFilters.Select(f => f.Accept(filterVisitor));
            var dataFilters = filter?.DataFilters.Select(f => MakeDataFilterDto(f));

            if (filters == null)
            {
                filters = dataFilters;
            }
            else if (dataFilters != null)
            {
                filters = filters.Concat(dataFilters);
            }

            return filters;
        }

        private static FilterDto MakeDataFilterDto(IDataFilter<TData> filter)
        {
            return new FilterDto()
            {
                FilterExpression = filter.DataFilter.Serialize(),
            };
        }

        private sealed class ColumnVisitor : IColumnVisitor<TData, ColumnDto>
        {
            public ColumnDto Visit<TColumn>(IColumn<TData, TColumn> column)
            {
                return new ColumnDto()
                {
                    Id = column.Id,
                    CanSort = column.CanSort,
                    CanFilter = column.CanFilter,
                    DataType = column.DataType.AssemblyQualifiedName,
                    DataGetterExpression = column.DataGetterExpression.Serialize(),
                };
            }
        }

        private sealed class FilterVisitor : IColumnFilterVisitor<TData, FilterDto>
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
                        DataGetterExpression = column.DataGetterExpression.Serialize(),
                    },
                    FilterExpression = columnFilter.Filter.Serialize(),
                };
            }
        }
    }
}
