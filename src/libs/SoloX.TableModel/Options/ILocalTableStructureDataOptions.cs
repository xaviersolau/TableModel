// ----------------------------------------------------------------------
// <copyright file="ILocalTableStructureDataOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq.Expressions;

namespace SoloX.TableModel.Options
{
    /// <summary>
    /// Local table structure data options.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TId">Table Id type.</typeparam>
    public interface ILocalTableStructureDataOptions<TData, TId>
    {
        /// <summary>
        /// Add column in table structure.
        /// </summary>
        /// <typeparam name="TColumn">Column value type.</typeparam>
        /// <param name="columnId">Column Id.</param>
        /// <param name="dataGetterExpression">Column value navigation expression.</param>
        /// <param name="header">Column header.</param>
        /// <param name="canSort">Tells if it can be sorted.</param>
        /// <param name="canFilter">Tells if it can be filtered.</param>
        /// <returns>The current table structure data options.</returns>
        ILocalTableStructureDataOptions<TData, TId> AddColumn<TColumn>(string columnId, Expression<Func<TData, TColumn>> dataGetterExpression, string? header = null, bool canSort = true, bool canFilter = true);

        /// <summary>
        /// Add column in table structure.
        /// </summary>
        /// <typeparam name="TColumn">Column value type.</typeparam>
        /// <param name="dataGetterExpression">Column value navigation expression.</param>
        /// <param name="header">Column header.</param>
        /// <param name="canSort">Tells if it can be sorted.</param>
        /// <param name="canFilter">Tells if it can be filtered.</param>
        /// <returns>The current table structure data options.</returns>
        /// <remarks>Expression property name is used as column ID.</remarks>
        ILocalTableStructureDataOptions<TData, TId> AddColumn<TColumn>(Expression<Func<TData, TColumn>> dataGetterExpression, string? header = null, bool canSort = true, bool canFilter = true);
    }
}
