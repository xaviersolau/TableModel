// ----------------------------------------------------------------------
// <copyright file="QueryableTableDataOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace SoloX.TableModel.Options.Impl
{
    /// <summary>
    /// Queriable table data options.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TQueryableTableData">Queriable table data type.</typeparam>
    public class QueryableTableDataOptions<TData, TQueryableTableData> : ATableDataOptions, IQueryableTableDataOptions<TData, TQueryableTableData>
        where TQueryableTableData : ITableData<TData>
    {
        /// <inheritdoc/>
        public Func<string, IServiceProvider, TQueryableTableData> Factory { get; set; }

        /// <summary>
        /// Setup QueryableTableDataOptions with Id.
        /// </summary>
        /// <param name="tableDataId">The table Id.</param>
        public QueryableTableDataOptions(string tableDataId) : base(tableDataId)
        {
        }

        /// <inheritdoc/>
        public override Task<ITableData> CreateModelInstanceAsync(IServiceProvider serviceProvider)
        {
            var tableData = Factory(TableDataId, serviceProvider);
            return Task.FromResult<ITableData>(tableData);
        }
    }
}