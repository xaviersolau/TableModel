// ----------------------------------------------------------------------
// <copyright file="IColumn.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq.Expressions;

namespace SoloX.TableModel
{
    /// <summary>
    /// Table Structure Column interface defining Visitor pattern and basic properties.
    /// </summary>
    /// <typeparam name="TData">Data type.</typeparam>
    public interface IColumn<TData>
    {
        /// <summary>
        /// Get the column Id.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Get the column Header.
        /// </summary>
        string? Header { get; }

        /// <summary>
        /// Tells if the column data can be sorted.
        /// </summary>
        bool CanSort { get; }

        /// <summary>
        /// Tells if the column data can be filtered.
        /// </summary>
        bool CanFilter { get; }

        /// <summary>
        /// Get the Column data type.
        /// </summary>
        Type DataType { get; }

        /// <summary>
        /// Get the column data value from the given table data item.
        /// </summary>
        /// <param name="data">The table data item to get the column data value from.</param>
        /// <returns>The generic column data value.</returns>
        object GetObject(TData data);

        /// <summary>
        /// Accept the given visitor and visit the actual typed table column.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Accept(IColumnVisitor<TData> visitor);

        /// <summary>
        /// Accept the given visitor and visit the actual typed table column to return the typed result.
        /// </summary>
        /// <typeparam name="TResult">The resulting type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>The visitor typed result.</returns>
        TResult Accept<TResult>(IColumnVisitor<TData, TResult> visitor);

        /// <summary>
        /// Accept the given visitor and visit the actual typed table column to return the typed result.
        /// </summary>
        /// <typeparam name="TResult">The resulting type.</typeparam>
        /// <typeparam name="TArg">The typed argument</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <param name="arg">The argument to transmit to the visitor.</param>
        /// <returns>The visitor typed result.</returns>
        TResult Accept<TResult, TArg>(IColumnVisitor<TData, TResult, TArg> visitor, TArg arg);
    }

    /// <summary>
    /// The typed table column interface.
    /// </summary>
    /// <typeparam name="TData">Data type.</typeparam>
    /// <typeparam name="TColumn">Column value type.</typeparam>
    public interface IColumn<TData, TColumn> : IColumn<TData>
    {
        /// <summary>
        /// Lambda expression to navigate to the column value.
        /// </summary>
        Expression<Func<TData, TColumn>> DataGetterExpression { get; }

        /// <summary>
        /// Get the column data value from the given table data item.
        /// </summary>
        /// <param name="data">The table data item to get the column data value from.</param>
        /// <returns>The typed column data value.</returns>
        TColumn GetValue(TData data);
    }
}
