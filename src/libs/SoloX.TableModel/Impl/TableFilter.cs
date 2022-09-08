// ----------------------------------------------------------------------
// <copyright file="TableFilter.cs" company="Xavier Solau">
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
    /// Table filter descriptor.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public class TableFilter<TData> : ITableFilter<TData>
    {
        private readonly List<IColumnFilter<TData>> columnFilters = new List<IColumnFilter<TData>>();
        private readonly List<IDataFilter<TData>> dataFilters = new List<IDataFilter<TData>>();

        ///<inheritdoc/>
        public IEnumerable<IColumnFilter<TData>> ColumnFilters => this.columnFilters;

        ///<inheritdoc/>
        public IEnumerable<IDataFilter<TData>> DataFilters => this.dataFilters;

        ///<inheritdoc/>
        public IQueryable<TData> Apply(IQueryable<TData> data)
        {
            var dataIter = data;

            foreach (var dataFilter in this.dataFilters)
            {
                dataIter = dataFilter.Apply(dataIter);
            }

            foreach (var columnFilter in this.columnFilters)
            {
                dataIter = columnFilter.Apply(dataIter);
            }

            return dataIter;
        }

        ///<inheritdoc/>
        public void Register<TColumn>(Expression<Func<TData, TColumn>> data, Expression<Func<TColumn, bool>> filter)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var propertyNameResolver = new PropertyNameResolver();

            var id = propertyNameResolver.GetPropertyName(data);

            Register(id, data, filter);
        }

        ///<inheritdoc/>
        public void Register<TColumn>(string columnId, Expression<Func<TData, TColumn>> data, Expression<Func<TColumn, bool>> filter)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Register(new Column<TData, TColumn>(columnId, data), filter);
        }

        ///<inheritdoc/>
        public void Register<TColumn>(IColumn<TData> column, Expression<Func<TColumn, bool>> filter)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (column is IColumn<TData, TColumn> typedColumn)
            {
                Register(typedColumn, filter);
            }
            else
            {
                throw new ArgumentException($"Incompatible column type {column.Id} [Type: {typeof(TColumn).Name}]");
            }
        }

        ///<inheritdoc/>
        public void Register<TColumn>(IColumn<TData, TColumn> column, Expression<Func<TColumn, bool>> filter)
        {
            this.columnFilters.Add(new ColumnFilter<TData, TColumn>(column, filter));
        }

        ///<inheritdoc/>
        public void Register(Expression<Func<TData, bool>> filter)
        {
            this.dataFilters.Add(new DataFilterBase<TData>(filter));
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
    }
}
