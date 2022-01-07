// ----------------------------------------------------------------------
// <copyright file="ITableStructure.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;

namespace SoloX.TableModel
{
    /// <summary>
    /// Table structure base interface.
    /// </summary>
    public interface ITableStructure
    {
        /// <summary>
        /// Get table structure Id.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Accept the given visitor and visit the actual typed table structure.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Accept(ITableStructureVisitor visitor);

        /// <summary>
        /// Accept the given visitor and visit the actual typed table structure to return the typed result.
        /// </summary>
        /// <typeparam name="TResult">The resulting type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        TResult Accept<TResult>(ITableStructureVisitor<TResult> visitor);

        /// <summary>
        /// Accept the given visitor and visit the actual typed table structure to return the typed result.
        /// </summary>
        /// <typeparam name="TResult">The resulting type.</typeparam>
        /// <typeparam name="TArg">The typed argument</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <param name="arg">The argument to transmit to the visitor.</param>
        TResult Accept<TResult, TArg>(ITableStructureVisitor<TResult, TArg> visitor, TArg arg);

    }

    /// <summary>
    /// Table structure typed interface.
    /// </summary>
    /// <typeparam name="TData">Data type the table structure is based on.</typeparam>
    public interface ITableStructure<TData> : ITableStructure
    {
        /// <summary>
        /// Get the table column descriptor matching the given id.
        /// </summary>
        /// <param name="id">The column id.</param>
        /// <returns>The matching table column.</returns>
        IColumn<TData> this[string id] { get; }

        /// <summary>
        /// Get all table columns (including Id column).
        /// </summary>
        IEnumerable<IColumn<TData>> Columns { get; }
    }

    /// <summary>
    /// Table structure fully typed interface.
    /// </summary>
    /// <typeparam name="TData">Data type the table structure is based on.</typeparam>
    /// <typeparam name="TId">Id type used to identify table data items.</typeparam>
    public interface ITableStructure<TData, TId> : ITableStructure<TData>
    {
        /// <summary>
        /// Get the column describing the table date item Id.
        /// </summary>
        IColumn<TData, TId> IdColumn { get; }

        /// <summary>
        /// Get the data table columns.
        /// </summary>
        IEnumerable<IColumn<TData>> DataColumns { get; }
    }
}
