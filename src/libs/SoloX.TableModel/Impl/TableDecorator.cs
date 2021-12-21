// ----------------------------------------------------------------------
// <copyright file="TableDecorator.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

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

        private Func<object, TDecorator>? defaultDecorator;

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
            TableStructure = tableStructure ?? throw new ArgumentNullException(nameof(tableStructure));
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        ///<inheritdoc/>
        public void RegisterDefault(Expression<Func<object, TDecorator>> decoratorExpression)
        {
            DefaultDecoratorExpression = decoratorExpression ?? throw new ArgumentNullException(nameof(decoratorExpression));
            this.defaultDecorator = decoratorExpression.Compile();
        }

        ///<inheritdoc/>
        public void Register<TColumn>(string columnId, Expression<Func<TColumn, TDecorator>> decoratorExpression)
        {
            var column = TableStructure[columnId];

            if (column is IColumn<TData, TColumn> typedColumn)
            {
                Register(typedColumn, decoratorExpression);
            }
            else
            {
                throw new ArgumentException($"Incompatible column type {column.Id} [Type: {typeof(TColumn).Name}]");
            }
        }

        ///<inheritdoc/>
        private void Register<TColumn>(IColumn<TData, TColumn> tableColumn, Expression<Func<TColumn, TDecorator>> relativeDecoratorExpression)
        {
            Register(new ColumnDecorator<TData, TDecorator, TColumn>(tableColumn, relativeDecoratorExpression));
        }

        ///<inheritdoc/>
        public void Register<TColumn>(IColumnDecorator<TData, TDecorator, TColumn> tableColumnDecorator)
        {
            if (tableColumnDecorator == null)
            {
                throw new ArgumentNullException(nameof(tableColumnDecorator));
            }

            var matchingColumn = TableStructure.Columns.FirstOrDefault(x => x.Id == tableColumnDecorator.Column.Id);

            if (matchingColumn == null)
            {
                throw new InvalidDataException(
                    $"Table column decorator {tableColumnDecorator.Column.Id} is not compatible with the current table structure {TableStructure.Id}.");
            }

            this.decoratorExpression.Add(tableColumnDecorator.Column.Id, tableColumnDecorator);
        }

        ///<inheritdoc/>
        public TDecorator Decorate(IColumn<TData> tableColumn, TData data)
        {
            if (tableColumn == null)
            {
                throw new ArgumentNullException(nameof(tableColumn));
            }

            if (this.decoratorExpression.TryGetValue(tableColumn.Id, out var columnDecorator))
            {
                return columnDecorator.Decorate(data);
            }

            return this.defaultDecorator != null ? this.defaultDecorator(tableColumn.GetObject(data)) : default;
        }

    }
}
