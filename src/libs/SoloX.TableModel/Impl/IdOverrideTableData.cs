// ----------------------------------------------------------------------
// <copyright file="IdOverrideTableData.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    internal class IdOverrideTableData<TData> : ITableData<TData>
    {
        private readonly ITableData<TData> tableData;
        public IdOverrideTableData(string id, ITableData<TData> tableData)
        {
            Id = id;
            this.tableData = tableData;
        }

        public string Id { get; }

        public bool DisableInstanceCaching => this.tableData.DisableInstanceCaching;

        public void Accept(ITableDataVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TResult Accept<TResult>(ITableDataVisitor<TResult> visitor)
        {
            return visitor.Visit(this);
        }

        public TResult Accept<TResult, TArg>(ITableDataVisitor<TResult, TArg> visitor, TArg arg)
        {
            return visitor.Visit(this, arg);
        }

        public Task<IEnumerable<TData>> GetDataAsync()
        {
            return this.tableData.GetDataAsync();
        }

        public Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter)
        {
            return this.tableData.GetDataAsync(sorting, filter);
        }

        public Task<IEnumerable<TData>> GetDataAsync(ITableFilter<TData> filter)
        {
            return this.tableData.GetDataAsync(filter);
        }

        public Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting)
        {
            return this.tableData.GetDataAsync(sorting);
        }

        public Task<int> GetDataCountAsync()
        {
            return this.tableData.GetDataCountAsync();
        }

        public Task<int> GetDataCountAsync(ITableFilter<TData> filter)
        {
            return this.tableData.GetDataCountAsync(filter);
        }

        public Task<IEnumerable<TData>> GetDataPageAsync(int offset, int pageSize)
        {
            return this.tableData.GetDataPageAsync(offset, pageSize);
        }

        public Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter, int offset, int pageSize)
        {
            return this.tableData.GetDataPageAsync(sorting, filter, offset, pageSize);
        }

        public Task<IEnumerable<TData>> GetDataPageAsync(ITableFilter<TData> filter, int offset, int pageSize)
        {
            return this.tableData.GetDataPageAsync(filter, offset, pageSize);
        }

        public Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, int offset, int pageSize)
        {
            return this.tableData.GetDataPageAsync(sorting, offset, pageSize);
        }
    }
}
