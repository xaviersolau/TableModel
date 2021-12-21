// ----------------------------------------------------------------------
// <copyright file="IColumnDecorator.cs" company="Xavier Solau">
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
    /// Table Column Decorator interface defining Visitor pattern and basic properties.
    /// </summary>
    /// <typeparam name="TData">Data type.</typeparam>
    /// <typeparam name="TDecorator">Decorator type.</typeparam>
    public interface IColumnDecorator<TData, TDecorator>
    {
        /// <summary>
        /// Get the associated column declaration.
        /// </summary>
        IColumn<TData> Column { get; }

        /// <summary>
        /// Get Lambda expression to navigate from the table data item to the decorated value.
        /// </summary>
        Expression<Func<TData, TDecorator>> AbsoluteDecoratorExpression { get; }

        /// <summary>
        /// Compute the decorated data from the table data item.
        /// </summary>
        /// <param name="data">The table data item to get the decorated value from.</param>
        /// <returns></returns>
        TDecorator Decorate(TData data);

        /// <summary>
        /// Accept the given visitor and visit the actual typed table column decorator.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Accept(IColumnDecoratorVisitor<TData, TDecorator> visitor);

        /// <summary>
        /// Accept the given visitor and visit the actual typed table column decorator to return the typed result.
        /// </summary>
        /// <typeparam name="TResult">The resulting type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>The visitor typed result.</returns>
        TResult Accept<TResult>(IColumnDecoratorVisitor<TData, TDecorator, TResult> visitor);

        /// <summary>
        /// Accept the given visitor and visit the actual typed table column decorator to return the typed result.
        /// </summary>
        /// <typeparam name="TResult">The resulting type.</typeparam>
        /// <typeparam name="TArg">The typed argument</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <param name="arg">The argument to transmit to the visitor.</param>
        /// <returns>The visitor typed result.</returns>
        TResult Accept<TResult, TArg>(IColumnDecoratorVisitor<TData, TDecorator, TResult, TArg> visitor, TArg arg);
    }

    /// <summary>
    /// The typed table decorator interface.
    /// </summary>
    /// <typeparam name="TData">Data type.</typeparam>
    /// <typeparam name="TDecorator">Decorator value type.</typeparam>
    /// <typeparam name="TColumn">Column value type.</typeparam>
    public interface IColumnDecorator<TData, TDecorator, TColumn> : IColumnDecorator<TData, TDecorator>
    {
        /// <summary>
        /// Get the associated typed column declaration.
        /// </summary>
        new IColumn<TData, TColumn> Column { get; }

        /// <summary>
        /// Get Lambda expression to navigate from the column value to the decorated value.
        /// </summary>
        Expression<Func<TColumn, TDecorator>> RelativeDecoratorExpression { get; }
    }
}
