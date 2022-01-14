// ----------------------------------------------------------------------
// <copyright file="ATableData.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// Table Data abstract base class.
    /// </summary>
    /// <typeparam name="TData">Provided Data type.</typeparam>
    public abstract class ATableData<TData> : ITableData<TData>
    {
        /// <summary>
        /// Setup Table Data with the given Id.
        /// </summary>
        /// <param name="id">The Id to use on this TableData.</param>
        protected ATableData(string id, bool disableInstanceCaching = false)
        {
            Id = id;
            DisableInstanceCaching = disableInstanceCaching;
        }

        ///<inheritdoc/>
        public string Id { get; internal set; }

        ///<inheritdoc/>
        public bool DisableInstanceCaching { get; }

        ///<inheritdoc/>
        public void Accept(ITableDataVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }

        ///<inheritdoc/>
        public TResult Accept<TResult>(ITableDataVisitor<TResult> visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            return visitor.Visit(this);
        }

        ///<inheritdoc/>
        public TResult Accept<TResult, TArg>(ITableDataVisitor<TResult, TArg> visitor, TArg arg)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            return visitor.Visit(this, arg);
        }

        ///<inheritdoc/>
        public abstract Task<IEnumerable<TData>> GetDataAsync();

        ///<inheritdoc/>
        public abstract Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter);

        ///<inheritdoc/>
        public abstract Task<IEnumerable<TData>> GetDataAsync(ITableFilter<TData> filter);

        ///<inheritdoc/>
        public abstract Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting);

        ///<inheritdoc/>
        public abstract Task<IEnumerable<TData>> GetDataPageAsync(int offset, int pageSize);

        ///<inheritdoc/>
        public abstract Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter, int offset, int pageSize);

        ///<inheritdoc/>
        public abstract Task<IEnumerable<TData>> GetDataPageAsync(ITableFilter<TData> filter, int offset, int pageSize);

        ///<inheritdoc/>
        public abstract Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, int offset, int pageSize);

        ///<inheritdoc/>
        public abstract Task<int> GetDataCountAsync();

        ///<inheritdoc/>
        public abstract Task<int> GetDataCountAsync(ITableFilter<TData> filter);

    }
}
