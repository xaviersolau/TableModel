// ----------------------------------------------------------------------
// <copyright file="ColumnDecorator.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using SoloX.ExpressionTools.Transform.Impl;

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// Column decorator descriptor.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TDecorator">Decorator data type.</typeparam>
    /// <typeparam name="TColumn">Column data type.</typeparam>
    public class ColumnDecorator<TData, TDecorator, TColumn> : IColumnDecorator<TData, TDecorator, TColumn>
    {
        private readonly Func<TData, TDecorator> decorator;
        private readonly Func<TDecorator>? headerDecorator;

        /// <summary>
        /// Setup a ColumnDecorator.
        /// </summary>
        /// <param name="column">The column to decorate.</param>
        /// <param name="relativeDecoratorExpression">Data Decorator expression from column value.</param>
        /// <param name="headerDecoratorExpression">Header Decorator expression.</param>
        public ColumnDecorator(IColumn<TData, TColumn> column,
            Expression<Func<TColumn, TDecorator>> relativeDecoratorExpression,
            Expression<Func<TDecorator>> headerDecoratorExpression)
        {
            Column = column ?? throw new ArgumentNullException(nameof(column));
            RelativeDecoratorExpression = relativeDecoratorExpression ?? throw new ArgumentNullException(nameof(relativeDecoratorExpression));

            HeaderDecoratorExpression = headerDecoratorExpression;

            // Value filter : (v) => v.ToString()
            // Column value : (d) => d.v
            // Resulting filter expression :
            // (d) => d.v.ToString()

            var inliner = new SingleParameterInliner();
            var absoluteDecoratorExpression = inliner.Amend(column.DataGetterExpression, relativeDecoratorExpression);

            AbsoluteDecoratorExpression = absoluteDecoratorExpression;

            this.decorator = absoluteDecoratorExpression.Compile();

            this.headerDecorator = headerDecoratorExpression?.Compile();
        }

        ///<inheritdoc/>
        public IColumn<TData, TColumn> Column { get; }

        ///<inheritdoc/>
        IColumn<TData> IColumnDecorator<TData, TDecorator>.Column => Column;

        ///<inheritdoc/>
        public Expression<Func<TColumn, TDecorator>> RelativeDecoratorExpression { get; }

        ///<inheritdoc/>
        public Expression<Func<TDecorator>>? HeaderDecoratorExpression { get; }

        ///<inheritdoc/>
        public Expression<Func<TData, TDecorator>> AbsoluteDecoratorExpression { get; }

        ///<inheritdoc/>
        public TDecorator? DecorateHeader()
        {
            return (this.headerDecorator != null) ? this.headerDecorator() : throw new NotSupportedException($"No header decorator expression defined for this column {Column.Id}");
        }

        ///<inheritdoc/>
        public TDecorator Decorate(TData data)
        {
            return this.decorator(data);
        }

        ///<inheritdoc/>
        public void Accept(IColumnDecoratorVisitor<TData, TDecorator> visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }

        ///<inheritdoc/>
        public TResult Accept<TResult>(IColumnDecoratorVisitor<TData, TDecorator, TResult> visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            return visitor.Visit(this);
        }

        ///<inheritdoc/>
        public TResult Accept<TResult, TArg>(IColumnDecoratorVisitor<TData, TDecorator, TResult, TArg> visitor, TArg arg)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            return visitor.Visit(this, arg);
        }
    }
}