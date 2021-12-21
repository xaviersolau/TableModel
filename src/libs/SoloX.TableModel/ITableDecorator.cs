// ----------------------------------------------------------------------
// <copyright file="ITableDecorator.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SoloX.TableModel
{
    /// <summary>
    /// Table decorator base interface.
    /// </summary>
    public interface ITableDecorator
    {
        /// <summary>
        /// Table decorator Id.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Table decorator type.
        /// </summary>
        Type DecoratorType { get; }
    }

    /// <summary>
    /// Table decorator fully typed interface.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TDecorator">Decorator type.</typeparam>
    public interface ITableDecorator<TData, TDecorator> : ITableDecorator
    {
        /// <summary>
        /// Get the associated table structure descriptor.
        /// </summary>
        ITableStructure<TData> TableStructure { get; }

        /// <summary>
        /// Get the default decorator expression.
        /// </summary>
        Expression<Func<object, TDecorator>> DefaultDecoratorExpression { get; }

        /// <summary>
        /// Get the column decorators.
        /// </summary>
        IEnumerable<IColumnDecorator<TData, TDecorator>> TableColumnDecorators { get; }

        /// <summary>
        /// Decorate the given data according to the given column.
        /// </summary>
        /// <param name="tableColumn">The table column to apply the decorator on.</param>
        /// <param name="data">The data to decorate.</param>
        /// <returns>The decorated table column data.</returns>
        TDecorator Decorate(IColumn<TData> tableColumn, TData data);
    }
}
