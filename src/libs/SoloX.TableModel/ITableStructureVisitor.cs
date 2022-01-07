// ----------------------------------------------------------------------
// <copyright file="ITableStructureVisitor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.TableModel
{
    /// <summary>
    /// Table Structure Visitor interface.
    /// </summary>
    public interface ITableStructureVisitor
    {
        /// <summary>
        /// Visit the typed table structure.
        /// </summary>
        /// <typeparam name="TData">Data type the table structure is based on.</typeparam>
        /// <typeparam name="TId">Id type used to identify table data items.</typeparam>
        /// <param name="tableStructure">The typed table structure.</param>
        void Visit<TData, TId>(ITableStructure<TData, TId> tableStructure);
    }

    /// <summary>
    /// Table Structure Visitor interface returning a typed result.
    /// </summary>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    public interface ITableStructureVisitor<TResult>
    {
        /// <summary>
        /// Visit the typed table structure.
        /// </summary>
        /// <typeparam name="TData">Data type the table structure is based on.</typeparam>
        /// <typeparam name="TId">Id type used to identify table data items.</typeparam>
        /// <param name="tableStructure">The typed table structure.</param>
        /// <returns>The visitor result.</returns>
        TResult Visit<TData, TId>(ITableStructure<TData, TId> tableStructure);
    }

    /// <summary>
    /// Table Structure Visitor interface returning a typed result.
    /// </summary>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <typeparam name="TArg">Type of the argument.</typeparam>
    public interface ITableStructureVisitor<TResult, TArg>
    {
        /// <summary>
        /// Visit the typed table structure.
        /// </summary>
        /// <typeparam name="TData">Data type the table structure is based on.</typeparam>
        /// <typeparam name="TId">Id type used to identify table data items.</typeparam>
        /// <param name="tableStructure">The typed table structure.</param>
        /// <param name="arg">The visitor argument.</param>
        /// <returns>The visitor result.</returns>
        TResult Visit<TData, TId>(ITableStructure<TData, TId> tableStructure, TArg arg);
    }

}