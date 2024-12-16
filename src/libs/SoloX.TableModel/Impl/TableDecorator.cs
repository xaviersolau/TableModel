// ----------------------------------------------------------------------
// <copyright file="TableDecorator.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#if NETSTANDARD2_1
using ArgumentNullException = SoloX.TableModel.Utils.ArgumentNullException;
#endif

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// Table decorator descriptor.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TDecorator">Decorator data type.</typeparam>
    public class TableDecorator<TData, TDecorator> : ITableDecorator<TData, TDecorator>
    {
        private readonly Dictionary<string, IColumnDecorator<TData, TDecorator>> decoratorExpression = new Dictionary<string, IColumnDecorator<TData, TDecorator>>();

        private Func<object?, TDecorator>? defaultDecorator;
        private Func<IColumn<TData>, TDecorator>? defaultHeaderDecorator;

        ///<inheritdoc/>
        public Expression<Func<IColumn<TData>, TDecorator>>? DefaultHeaderDecoratorExpression { get; private set; }

        ///<inheritdoc/>
        public Expression<Func<object, TDecorator>>? DefaultDecoratorExpression { get; private set; }

        ///<inheritdoc/>
        public IEnumerable<IColumnDecorator<TData, TDecorator>> TableColumnDecorators => this.decoratorExpression.Values;

        ///<inheritdoc/>
        public ITableStructure<TData> TableStructure { get; }

        ///<inheritdoc/>
        public string Id { get; }

        ///<inheritdoc/>
        public Type DecoratorType => typeof(TDecorator);

        /// <summary>
        /// Setup a TableDecorator for the given table structure.
        /// </summary>
        /// <param name="id">Decorator Id.</param>
        /// <param name="tableStructure">Table structure to decorate.</param>
        public TableDecorator(string id, ITableStructure<TData> tableStructure)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(tableStructure, nameof(tableStructure));

            TableStructure = tableStructure;
            Id = id;
        }

        /// <summary>
        /// Register default table decorator.
        /// </summary>
        /// <param name="decoratorExpression">Default data decorator expression.</param>
        /// <param name="headerDecoratorExpression">Default header decorator expression.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void RegisterDefault(Expression<Func<object, TDecorator>> decoratorExpression, Expression<Func<IColumn<TData>, TDecorator>> headerDecoratorExpression)
        {
            ArgumentNullException.ThrowIfNull(decoratorExpression, nameof(decoratorExpression));
            ArgumentNullException.ThrowIfNull(headerDecoratorExpression, nameof(headerDecoratorExpression));

            DefaultDecoratorExpression = decoratorExpression;
            this.defaultDecorator = decoratorExpression.Compile();

            DefaultHeaderDecoratorExpression = headerDecoratorExpression;
            this.defaultHeaderDecorator = headerDecoratorExpression.Compile();
        }

        /// <summary>
        /// Try to register a column decorator.
        /// </summary>
        /// <typeparam name="TColumn"></typeparam>
        /// <param name="columnId"></param>
        /// <param name="decoratorExpression"></param>
        /// <param name="headerDecoratorExpression"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool TryRegister<TColumn>(string columnId, Expression<Func<TColumn, TDecorator>> decoratorExpression, Expression<Func<TDecorator>> headerDecoratorExpression)
        {
            var column = TableStructure.Columns.FirstOrDefault(x => x.Id == columnId);

            if (column == null)
            {
                return false;
            }

            if (column is IColumn<TData, TColumn> typedColumn)
            {
                return TryRegister(typedColumn, decoratorExpression, headerDecoratorExpression);
            }
            else
            {
                throw new ArgumentException($"Incompatible column type {column.Id} [Type: {typeof(TColumn).Name}]");
            }
        }

        private bool TryRegister<TColumn>(IColumn<TData, TColumn> tableColumn, Expression<Func<TColumn, TDecorator>> relativeDecoratorExpression, Expression<Func<TDecorator>> headerDecoratorExpression)
        {
            return TryRegister(new ColumnDecorator<TData, TDecorator, TColumn>(tableColumn, relativeDecoratorExpression, headerDecoratorExpression));
        }

        ///<inheritdoc/>
        public bool TryRegister<TColumn>(IColumnDecorator<TData, TDecorator, TColumn> tableColumnDecorator)
        {
            ArgumentNullException.ThrowIfNull(tableColumnDecorator, nameof(tableColumnDecorator));

            var matchingColumn = TableStructure.Columns.FirstOrDefault(x => x.Id == tableColumnDecorator.Column.Id);

            if (matchingColumn == null)
            {
                return false;
            }

            this.decoratorExpression.Add(tableColumnDecorator.Column.Id, tableColumnDecorator);
            return true;
        }

        ///<inheritdoc/>
        public TDecorator Decorate(IColumn<TData> tableColumn, TData data)
        {
            ArgumentNullException.ThrowIfNull(tableColumn, nameof(tableColumn));

            if (this.decoratorExpression.TryGetValue(tableColumn.Id, out var columnDecorator))
            {
                return columnDecorator.Decorate(data);
            }

            return this.defaultDecorator != null ? this.defaultDecorator(tableColumn.GetObject(data)) : default!;
        }

        ///<inheritdoc/>
        public TDecorator DecorateHeader(IColumn<TData> tableColumn)
        {
            ArgumentNullException.ThrowIfNull(tableColumn, nameof(tableColumn));

            if (this.decoratorExpression.TryGetValue(tableColumn.Id, out var columnDecorator))
            {
                if (columnDecorator.HeaderDecoratorExpression != null)
                {
                    return columnDecorator.DecorateHeader();
                }
            }

            return this.defaultHeaderDecorator != null ? this.defaultHeaderDecorator(tableColumn) : default!;
        }
    }
}
