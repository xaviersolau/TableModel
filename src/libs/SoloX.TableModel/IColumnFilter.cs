// ----------------------------------------------------------------------
// <copyright file="IColumnFilter.cs" company="Xavier Solau">
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
    /// Table Column filter interface defining Visitor pattern and basic properties.
    /// </summary>
    /// <typeparam name="TData">Table Data type.</typeparam>
    public interface IColumnFilter<TData> : IDataFilter<TData>
    {
        /// <summary>
        /// Get the associated column declaration.
        /// </summary>
        IColumn<TData> Column { get; }

        /// <summary>
        /// Accept the given visitor and visit the actual typed column filter.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Accept(IColumnFilterVisitor<TData> visitor);

        /// <summary>
        /// Accept the given visitor and visit the actual typed column filter to return the typed result.
        /// </summary>
        /// <typeparam name="TResult">The resulting type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>The visitor typed result.</returns>
        TResult Accept<TResult>(IColumnFilterVisitor<TData, TResult> visitor);

        /// <summary>
        /// Accept the given visitor and visit the actual typed column filter to return the typed result.
        /// </summary>
        /// <typeparam name="TResult">The resulting type.</typeparam>
        /// <typeparam name="TArg">The typed argument</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <param name="arg">The argument to transmit to the visitor.</param>
        /// <returns>The visitor typed result.</returns>
        TResult Accept<TResult, TArg>(IColumnFilterVisitor<TData, TResult, TArg> visitor, TArg arg);
    }

    /// <summary>
    /// The typed table column filter.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TColumn"></typeparam>
    public interface IColumnFilter<TData, TColumn> : IColumnFilter<TData>
    {
        /// <summary>
        /// Get the associated typed column declaration.
        /// </summary>
        new IColumn<TData, TColumn> Column { get; }

        /// <summary>
        /// Lambda expression defining the filter to apply to the column value.
        /// </summary>
        Expression<Func<TColumn, bool>> Filter { get; }
    }
}
