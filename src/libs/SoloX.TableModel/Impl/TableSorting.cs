// ----------------------------------------------------------------------
// <copyright file="TableSorting.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.ExpressionTools.Transform.Impl.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// Table sorting descriptor.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public class TableSorting<TData> : ITableSorting<TData>
    {
        private readonly List<IColumnSorting<TData>> columnSortings = new List<IColumnSorting<TData>>();

        ///<inheritdoc/>
        public IEnumerable<IColumnSorting<TData>> ColumnSortings => this.columnSortings;

        ///<inheritdoc/>
        public IQueryable<TData> Apply(IQueryable<TData> data)
        {
            var dataIter = data;

            foreach (var columnSorting in this.columnSortings)
            {
                dataIter = columnSorting.Apply(dataIter);
            }

            return dataIter;
        }

        ///<inheritdoc/>
        public void Register<TColumn>(Expression<Func<TData, TColumn>> data, SortingOrder order)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var propertyNameResolver = new PropertyNameResolver();

            var id = propertyNameResolver.GetPropertyName(data);

            Register(id, data, order);
        }

        ///<inheritdoc/>
        public void Register<TColumn>(string columnId, Expression<Func<TData, TColumn>> data, SortingOrder order)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Register(new Column<TData, TColumn>(columnId, data), order);
        }

        ///<inheritdoc/>
        public void Register(IColumn<TData> column, SortingOrder order)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            column.Accept(new Visitor(order, this.columnSortings));
        }

        ///<inheritdoc/>
        public void UnRegister(IColumn<TData> column)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public void UnRegister(string columnId)
        {
            throw new NotImplementedException();
        }

        private class Visitor : IColumnVisitor<TData>
        {
            private readonly SortingOrder order;
            private readonly List<IColumnSorting<TData>> columnSortings;

            public Visitor(SortingOrder order, List<IColumnSorting<TData>> columnSortings)
            {
                this.order = order;
                this.columnSortings = columnSortings;
            }

            public void Visit<TColumn>(IColumn<TData, TColumn> column)
            {
                this.columnSortings.Add(new ColumnSorting<TData, TColumn>(column, this.order));
            }
        }
    }
}
