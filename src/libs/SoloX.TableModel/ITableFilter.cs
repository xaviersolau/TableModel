// ----------------------------------------------------------------------
// <copyright file="ITableFilter.cs" company="Xavier Solau">
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
    /// Table filter descriptor.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public interface ITableFilter<TData>
    {
        /// <summary>
        /// Get the column filters.
        /// </summary>
        IEnumerable<IColumnFilter<TData>> ColumnFilters { get; }

        /// <summary>
        /// Get the data filters.
        /// </summary>
        IEnumerable<IDataFilter<TData>> DataFilters { get; }

        /// <summary>
        /// Register a filter column from the given column data expression.
        /// </summary>
        /// <typeparam name="TColumn">Column data type.</typeparam>
        /// <param name="data">The expression driving to the column data.</param>
        /// <param name="filter">The filter operation expression.</param>
        void Register<TColumn>(Expression<Func<TData, TColumn>> data, Expression<Func<TColumn, bool>> filter);

        /// <summary>
        /// Register a filter column from the given column data expression.
        /// </summary>
        /// <typeparam name="TColumn">Column data type.</typeparam>
        /// <param name="columnId">The column Id.</param>
        /// <param name="data">The expression driving to the column data.</param>
        /// <param name="filter">The filter operation expression.</param>
        void Register<TColumn>(string columnId, Expression<Func<TData, TColumn>> data, Expression<Func<TColumn, bool>> filter);

        /// <summary>
        /// Register a filter column.
        /// </summary>
        /// <typeparam name="TColumn">Column data type.</typeparam>
        /// <param name="column">The column to filter.</param>
        /// <param name="filter">The filter operation expression.</param>
        void Register<TColumn>(IColumn<TData> column, Expression<Func<TColumn, bool>> filter);

        /// <summary>
        /// Register a filter column.
        /// </summary>
        /// <typeparam name="TColumn">Column data type.</typeparam>
        /// <param name="column">The column to filter.</param>
        /// <param name="filter">The filter operation expression.</param>
        void Register<TColumn>(IColumn<TData, TColumn> column, Expression<Func<TColumn, bool>> filter);

        /// <summary>
        /// Register a data filter.
        /// </summary>
        /// <param name="filter">The filter operation expression.</param>
        void Register(Expression<Func<TData, bool>> filter);

        /// <summary>
        /// Unregister the column from filters.
        /// </summary>
        /// <param name="column">The column to unregister.</param>
        void UnRegister(IColumn<TData> column);

        /// <summary>
        /// Unregister the column matching the given Id from filters.
        /// </summary>
        /// <param name="columnId">The column Id to unregister.</param>
        void UnRegister(string columnId);

        /// <summary>
        /// Apply the filter on the given data.
        /// </summary>
        /// <param name="data">The data to apply the filter on.</param>
        /// <returns>The filtered data.</returns>
        IQueryable<TData> Apply(IQueryable<TData> data);
    }
}
