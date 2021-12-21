// ----------------------------------------------------------------------
// <copyright file="InMemoryTableData.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// In memory table data provider.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public class InMemoryTableData<TData> : AQueryableTableData<TData>
    {
        private readonly IEnumerable<TData> data;

        /// <summary>
        /// Setup a InMemoryTableData with the given data.
        /// </summary>
        /// <param name="id">Table data Id.</param>
        /// <param name="data">In memory data to provide.</param>
        public InMemoryTableData(string id, IEnumerable<TData> data)
            : base(id)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
        }

        ///<inheritdoc/>
        protected override IQueryable<TData> QueryData()
        {
            return this.data.AsQueryable();
        }
    }
}
