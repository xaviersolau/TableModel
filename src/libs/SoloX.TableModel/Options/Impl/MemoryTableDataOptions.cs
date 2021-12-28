// ----------------------------------------------------------------------
// <copyright file="MemoryTableDataOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoloX.TableModel.Impl;

namespace SoloX.TableModel.Options.Impl
{
    /// <summary>
    /// In memory table data options.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public class MemoryTableDataOptions<TData> : ATableDataOptions, IMemoryTableDataOptions<TData>
    {
        private IEnumerable<TData> data;

        /// <summary>
        /// Setup In memory table data options.
        /// </summary>
        /// <param name="tableId">Table data Id.</param>
        public MemoryTableDataOptions(string tableId)
            : base(tableId, typeof(TData))
        {
        }

        /// <inheritdoc/>
        public void AddData(IEnumerable<TData> data)
        {
            this.data = data;
        }

        /// <inheritdoc/>
        public override Task<ITableData> CreateModelInstanceAsync(IServiceProvider serviceProvider)
        {
            var tableData = new InMemoryTableData<TData>(TableDataId, this.data);
            return Task.FromResult<ITableData>(tableData);
        }
    }
}
