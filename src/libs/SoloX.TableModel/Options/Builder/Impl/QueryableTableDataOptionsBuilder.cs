// ----------------------------------------------------------------------
// <copyright file="QueryableTableDataOptionsBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using SoloX.TableModel.Options.Impl;

namespace SoloX.TableModel.Options.Builder.Impl
{
    /// <summary>
    /// Queriable table data options builder.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TQueryableTableData">Queriable table data type.</typeparam>
    public class QueryableTableDataOptionsBuilder<TData, TQueryableTableData> : ATableDataOptionsBuilder, IQueryableTableDataOptionsBuilder<TData, TQueryableTableData>
        where TQueryableTableData : ITableData<TData>
    {
        private readonly Action<IQueryableTableDataOptions<TData, TQueryableTableData>> tableDataOptionsSetup;

        /// <summary>
        /// Setup QueryableTableDataOptionsBuilder instance with Id and setup delegate.
        /// </summary>
        /// <param name="tableDataId">Table data Id.</param>
        /// <param name="tableDataOptionsSetup">Setup delegate.</param>
        public QueryableTableDataOptionsBuilder(string tableDataId, Action<IQueryableTableDataOptions<TData, TQueryableTableData>> tableDataOptionsSetup) : base(tableDataId)
        {
            this.tableDataOptionsSetup = tableDataOptionsSetup;
        }

        /// <inheritdoc/>
        public override ATableDataOptions Build()
        {
            var opt = new QueryableTableDataOptions<TData, TQueryableTableData>(TableDataId);

            this.tableDataOptionsSetup(opt);

            return opt;
        }
    }
}