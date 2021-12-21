// ----------------------------------------------------------------------
// <copyright file="ITableSorting.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SoloX.TableModel
{
    /// <summary>
    /// Table sorting descriptor interface.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public interface ITableSorting<TData>
    {
        /// <summary>
        /// Get registered sorting columns.
        /// </summary>
        IEnumerable<IColumnSorting<TData>> ColumnSortings { get; }

        /// <summary>
        /// Register a sorting column from the given column data expression.
        /// </summary>
        /// <typeparam name="TColumn">Column data type.</typeparam>
        /// <param name="data">The expression driving to the column data.</param>
        /// <param name="order">The sorting order.</param>
        void Register<TColumn>(Expression<Func<TData, TColumn>> data, SortingOrder order);

        /// <summary>
        /// Register a sorting column from the given column data expression.
        /// </summary>
        /// <typeparam name="TColumn">Column data type.</typeparam>
        /// <param name="columnId">The column Id.</param>
        /// <param name="data">The expression driving to the column data.</param>
        /// <param name="order">The sorting order.</param>
        void Register<TColumn>(string columnId, Expression<Func<TData, TColumn>> data, SortingOrder order);

        /// <summary>
        /// Register a sorting column.
        /// </summary>
        /// <param name="column">The column to sort.</param>
        /// <param name="order">The sorting order.</param>
        void Register(IColumn<TData> column, SortingOrder order);

        /// <summary>
        /// Unregister the column from sorting.
        /// </summary>
        /// <param name="column">The column to unregister.</param>
        void UnRegister(IColumn<TData> column);

        /// <summary>
        /// Unregister the column matching the given Id from sorting.
        /// </summary>
        /// <param name="columnId">The column Id to unregister.</param>
        void UnRegister(string columnId);

        /// <summary>
        /// Apply the sorting on the given data.
        /// </summary>
        /// <param name="data">The data to apply the sorting on.</param>
        /// <returns>The sorted data.</returns>
        IQueryable<TData> Apply(IQueryable<TData> data);
    }
}
