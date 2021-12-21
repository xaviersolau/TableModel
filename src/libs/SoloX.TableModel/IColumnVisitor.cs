// ----------------------------------------------------------------------
// <copyright file="IColumnVisitor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.TableModel
{
    /// <summary>
    /// Table Column Visitor interface.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public interface IColumnVisitor<TData>
    {
        /// <summary>
        /// Visit the typed table column.
        /// </summary>
        /// <typeparam name="TColumn">Column value type.</typeparam>
        /// <param name="column">The typed column.</param>
        void Visit<TColumn>(IColumn<TData, TColumn> column);
    }

    /// <summary>
    /// Table Column Visitor interface returning a typed result.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    public interface IColumnVisitor<TData, TResult>
    {
        /// <summary>
        /// Visit the typed table column.
        /// </summary>
        /// <typeparam name="TColumn">Column value type.</typeparam>
        /// <param name="column">The typed column.</param>
        /// <returns>The visitor result.</returns>
        TResult Visit<TColumn>(IColumn<TData, TColumn> column);
    }

    /// <summary>
    /// Table Column Visitor interface returning a typed result.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <typeparam name="TArg">Type of the argument.</typeparam>
    public interface IColumnVisitor<TData, TResult, TArg>
    {
        /// <summary>
        /// Visit the typed table column.
        /// </summary>
        /// <typeparam name="TColumn">Column value type.</typeparam>
        /// <param name="column">The typed column.</param>
        /// <param name="arg">The visitor argument.</param>
        /// <returns>The visitor result.</returns>
        TResult Visit<TColumn>(IColumn<TData, TColumn> column, TArg arg);
    }
}
