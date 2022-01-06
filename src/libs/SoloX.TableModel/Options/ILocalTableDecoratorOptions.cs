// ----------------------------------------------------------------------
// <copyright file="ILocalTableDecoratorOptions.cs" company="Xavier Solau">
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
    /// Local table decorator data options.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TDecorator">Decorator value type.</typeparam>
    public interface ILocalTableDecoratorOptions<TData, TDecorator>
    {
        /// <summary>
        /// Set the default column decorator.
        /// </summary>
        /// <param name="defaultDecoratorExpression">Default lambda expression to compute decorated value.</param>
        /// <param name="defaultHeaderDecoratorExpression">Default lambda expression to compute decorated header.</param>
        /// <returns>The current table decorator data options.</returns>
        ILocalTableDecoratorDataOptions<TData, TDecorator> AddDefault(
            Expression<Func<object, TDecorator>> defaultDecoratorExpression,
            Expression<Func<IColumn<TData>, TDecorator>> defaultHeaderDecoratorExpression);
    }
}
