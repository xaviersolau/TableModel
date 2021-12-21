// ----------------------------------------------------------------------
// <copyright file="AQueryableTableData.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// Abstract queryable table data providing a base implementation (to be used with a EF data source for instance).
    /// </summary>
    /// <typeparam name="TData">The table data type.</typeparam>
    public abstract class AQueryableTableData<TData> : ATableData<TData>
    {
        /// <summary>
        /// Setup the AQueryableTableData.
        /// </summary>
        /// <param name="id">Id to identify the table data.</param>
        protected AQueryableTableData(string id)
            : base(id, true)
        {
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataAsync()
        {
            var request = QueryData();

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (sorting == null)
            {
                throw new ArgumentNullException(nameof(sorting));
            }

            var request = QueryData();

            request = filter.Apply(request);

            request = sorting.Apply(request);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataAsync(ITableFilter<TData> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var request = QueryData();

            request = filter.Apply(request);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting)
        {
            if (sorting == null)
            {
                throw new ArgumentNullException(nameof(sorting));
            }

            var request = QueryData();

            request = sorting.Apply(request);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataPageAsync(int offset, int pageSize)
        {
            var request = QueryData();

            request = request.Skip(offset).Take(pageSize);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter, int offset, int pageSize)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (sorting == null)
            {
                throw new ArgumentNullException(nameof(sorting));
            }

            var request = QueryData();

            request = filter.Apply(request);

            request = sorting.Apply(request);

            request = request.Skip(offset).Take(pageSize);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableFilter<TData> filter, int offset, int pageSize)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var request = QueryData();

            request = filter.Apply(request);

            request = request.Skip(offset).Take(pageSize);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, int offset, int pageSize)
        {
            if (sorting == null)
            {
                throw new ArgumentNullException(nameof(sorting));
            }

            var request = QueryData();

            request = sorting.Apply(request);

            request = request.Skip(offset).Take(pageSize);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        ///<inheritdoc/>
        public override Task<int> GetDataCountAsync()
        {
            var request = QueryData();

            return Task.FromResult(request.Count());
        }

        ///<inheritdoc/>
        public override Task<int> GetDataCountAsync(ITableFilter<TData> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var request = QueryData();

            request = filter.Apply(request);

            return Task.FromResult(request.Count());
        }

        /// <summary>
        /// Provide the actual Data queryable instance.
        /// </summary>
        /// <returns>The queryable instance to load the data from.</returns>
        protected abstract IQueryable<TData> QueryData();
    }
}
