// ----------------------------------------------------------------------
// <copyright file="ILocalTableStructureOptions.cs" company="Xavier Solau">
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
    /// Local table structure options.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TId">Table Id type.</typeparam>
    public interface ILocalTableStructureOptions<TData, TId>
    {
        /// <summary>
        /// Add Id column in table structure.
        /// </summary>
        /// <typeparam name="TId">Column Id type.</typeparam>
        /// <param name="columnId">Column Id.</param>
        /// <param name="idGetterExpression">Column Id value navigation expression.</param>
        /// <param name="canSort">Tells if it can be sorted.</param>
        /// <param name="canFilter">Tells if it can be filtered.</param>
        /// <returns>The current table structure options.</returns>
        ILocalTableStructureDataOptions<TData, TId> AddIdColumn(string columnId, Expression<Func<TData, TId>> idGetterExpression, bool canSort = true, bool canFilter = true);

        /// <summary>
        /// Add Id column in table structure.
        /// </summary>
        /// <typeparam name="TId">Column Id type.</typeparam>
        /// <param name="idGetterExpression">Column Id value navigation expression.</param>
        /// <param name="canSort">Tells if it can be sorted.</param>
        /// <param name="canFilter">Tells if it can be filtered.</param>
        /// <returns>The current table structure options.</returns>
        /// <remarks>Expression property name is used as column ID.</remarks>
        ILocalTableStructureDataOptions<TData, TId> AddIdColumn(Expression<Func<TData, TId>> idGetterExpression, bool canSort = true, bool canFilter = true);
    }
}
