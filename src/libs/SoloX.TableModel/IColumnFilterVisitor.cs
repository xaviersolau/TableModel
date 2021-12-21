// ----------------------------------------------------------------------
// <copyright file="IColumnFilterVisitor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.TableModel
{
    /// <summary>
    /// Table Column Filter Visitor interface.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public interface IColumnFilterVisitor<TData>
    {
        /// <summary>
        /// Visit the typed table data filter.
        /// </summary>
        /// <typeparam name="TColumn">Column value type.</typeparam>
        /// <param name="columnFilter">The typed column filter.</param>
        void Visit<TColumn>(IColumnFilter<TData, TColumn> columnFilter);
    }

    /// <summary>
    /// Table Column Filter Visitor interface returning a typed result.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    public interface IColumnFilterVisitor<TData, TResult>
    {
        /// <summary>
        /// Visit the typed table data filter.
        /// </summary>
        /// <typeparam name="TColumn">Column value type.</typeparam>
        /// <param name="columnFilter">The typed column filter.</param>
        /// <returns>The visitor result.</returns>
        TResult Visit<TColumn>(IColumnFilter<TData, TColumn> columnFilter);
    }

    /// <summary>
    /// Table Column Filter Visitor interface returning a typed result.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <typeparam name="TArg">Type of the argument.</typeparam>
    public interface IColumnFilterVisitor<TData, TResult, TArg>
    {
        /// <summary>
        /// Visit the typed table data filter.
        /// </summary>
        /// <typeparam name="TColumn">Column value type.</typeparam>
        /// <param name="columnFilter">The typed column filter.</param>
        /// <param name="arg">The visitor argument.</param>
        /// <returns>The visitor result.</returns>
        TResult Visit<TColumn>(IColumnFilter<TData, TColumn> columnFilter, TArg arg);
    }
}
