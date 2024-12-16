// ----------------------------------------------------------------------
// <copyright file="AQueryableTableData.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if NETSTANDARD2_1
using ArgumentNullException = SoloX.TableModel.Utils.ArgumentNullException;
#else
using System;
#endif

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
        protected AQueryableTableData()
            : base(typeof(TData).FullName, true)
        {
        }

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

            request = ApplyPostFiltering(request);

            return ApplyPostProcessingAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter)
        {
            ArgumentNullException.ThrowIfNull(filter, nameof(filter));
            ArgumentNullException.ThrowIfNull(sorting, nameof(sorting));

            var request = QueryData();

            request = filter.Apply(request);

            request = ApplyPostFiltering(request);

            request = sorting.Apply(request);

            return ApplyPostProcessingAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataAsync(ITableFilter<TData> filter)
        {
            ArgumentNullException.ThrowIfNull(filter, nameof(filter));

            var request = QueryData();

            request = filter.Apply(request);

            request = ApplyPostFiltering(request);

            return ApplyPostProcessingAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting)
        {
            ArgumentNullException.ThrowIfNull(sorting, nameof(sorting));

            var request = QueryData();

            request = ApplyPostFiltering(request);

            request = sorting.Apply(request);

            return ApplyPostProcessingAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataPageAsync(int offset, int pageSize)
        {
            var request = QueryData();

            request = ApplyPostFiltering(request);

            request = request.Skip(offset).Take(pageSize);

            return ApplyPostProcessingAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter, int offset, int pageSize)
        {
            ArgumentNullException.ThrowIfNull(filter, nameof(filter));
            ArgumentNullException.ThrowIfNull(sorting, nameof(sorting));

            var request = QueryData();

            request = filter.Apply(request);

            request = ApplyPostFiltering(request);

            request = sorting.Apply(request);

            request = request.Skip(offset).Take(pageSize);

            return ApplyPostProcessingAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableFilter<TData> filter, int offset, int pageSize)
        {
            ArgumentNullException.ThrowIfNull(filter, nameof(filter));

            var request = QueryData();

            request = filter.Apply(request);

            request = ApplyPostFiltering(request);

            request = request.Skip(offset).Take(pageSize);

            return ApplyPostProcessingAsync(request);
        }

        ///<inheritdoc/>
        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, int offset, int pageSize)
        {
            ArgumentNullException.ThrowIfNull(sorting, nameof(sorting));

            var request = QueryData();

            request = ApplyPostFiltering(request);

            request = sorting.Apply(request);

            request = request.Skip(offset).Take(pageSize);

            return ApplyPostProcessingAsync(request);
        }

        ///<inheritdoc/>
        public override Task<int> GetDataCountAsync()
        {
            var request = QueryData();

            request = ApplyPostFiltering(request);

            return Task.FromResult(request.Count());
        }

        ///<inheritdoc/>
        public override Task<int> GetDataCountAsync(ITableFilter<TData> filter)
        {
            ArgumentNullException.ThrowIfNull(filter, nameof(filter));

            var request = QueryData();

            request = filter.Apply(request);

            request = ApplyPostFiltering(request);

            return Task.FromResult(request.Count());
        }

        /// <summary>
        /// Provide the actual Data queryable instance.
        /// </summary>
        /// <returns>The queryable instance to load the data from.</returns>
        protected abstract IQueryable<TData> QueryData();

        /// <summary>
        /// Apply an optional post filtering on the data.
        /// </summary>
        /// <returns>The post filtered data.</returns>
        protected virtual IQueryable<TData> ApplyPostFiltering(IQueryable<TData> data)
        {
            return data;
        }

        /// <summary>
        /// Apply an optional post processing on the data.
        /// </summary>
        /// <returns>The post processed data.</returns>
        protected virtual Task<IEnumerable<TData>> ApplyPostProcessingAsync(IQueryable<TData> data)
        {
            return Task.FromResult<IEnumerable<TData>>(data);
        }
    }
}
