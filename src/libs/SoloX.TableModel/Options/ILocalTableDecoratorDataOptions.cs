// ----------------------------------------------------------------------
// <copyright file="ILocalTableDecoratorDataOptions.cs" company="Xavier Solau">
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
    public interface ILocalTableDecoratorDataOptions<TData, TDecorator>
    {
        /// <summary>
        /// Add a new column decorator.
        /// </summary>
        /// <typeparam name="TColumn">Column value type.</typeparam>
        /// <param name="columnId">Column Id.</param>
        /// <param name="headerDecoratorExpression">Lambda expression to compute decorated header.</param>
        /// <param name="decoratorExpression">Lambda expression to compute decorated value.</param>
        /// <returns>The current table decorator data options.</returns>
        ILocalTableDecoratorDataOptions<TData, TDecorator> Add<TColumn>(
            string columnId,
            Expression<Func<TColumn, TDecorator>> decoratorExpression,
            Expression<Func<TDecorator>>? headerDecoratorExpression = null);

        /// <summary>
        /// Add a new column decorator.
        /// </summary>
        /// <typeparam name="TColumn">Column value type.</typeparam>
        /// <param name="columnPropertyExpression">Column property expression.</param>
        /// <param name="decoratorExpression">Lambda expression to compute decorated value.</param>
        /// <param name="headerDecoratorExpression">Lambda expression to compute decorated header.</param>
        /// <returns>The current table decorator data options.</returns>
        /// <remarks>Column expression property name is used as column Id.</remarks>
        ILocalTableDecoratorDataOptions<TData, TDecorator> Add<TColumn>(
            Expression<Func<TData, TColumn>> columnPropertyExpression,
            Expression<Func<TColumn, TDecorator>> decoratorExpression,
            Expression<Func<TDecorator>>? headerDecoratorExpression = null);
    }
}
