// ----------------------------------------------------------------------
// <copyright file="ITableDataVisitor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.TableModel
{
    /// <summary>
    /// Base table data visitor interface.
    /// </summary>
    public interface ITableDataVisitor
    {
        /// <summary>
        /// Visit the typed table data.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <param name="tableData">Typed table data object.</param>
        void Visit<TData>(ITableData<TData> tableData);
    }

    /// <summary>
    /// Table data visitor interface returning a typed result.
    /// </summary>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    public interface ITableDataVisitor<TResult>
    {
        /// <summary>
        /// Visit the typed table data.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <param name="tableData">Typed table data object.</param>
        /// <returns>A visitor result.</returns>
        TResult Visit<TData>(ITableData<TData> tableData);
    }

    /// <summary>
    /// Table data visitor interface returning a typed result and taking a typed argument.
    /// </summary>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <typeparam name="TArg">Type of the argument.</typeparam>
    public interface ITableDataVisitor<TResult, TArg>
    {
        /// <summary>
        /// Visit the typed table data.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <param name="tableData">Typed table data object.</param>
        /// <param name="arg">The visitor argument.</param>
        /// <returns>A visitor result.</returns>
        TResult Visit<TData>(ITableData<TData> tableData, TArg arg);
    }
}
