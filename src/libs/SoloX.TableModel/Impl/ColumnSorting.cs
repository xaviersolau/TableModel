// ----------------------------------------------------------------------
// <copyright file="ColumnSorting.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// Column sorting descriptor.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TColumn">Column data type.</typeparam>
    public class ColumnSorting<TData, TColumn> : IColumnSorting<TData>
    {
        ///<inheritdoc/>
        public IColumn<TData> Column { get; }

        ///<inheritdoc/>
        public SortingOrder Order { get; }

        ///<inheritdoc/>
        private Expression<Func<TData, TColumn>> DataGetter { get; }

        /// <summary>
        /// Setup a ColumnSorting 
        /// </summary>
        /// <param name="column">Column to apply the sorting operation on.</param>
        /// <param name="order">Sorting order to use.</param>
        public ColumnSorting(IColumn<TData, TColumn> column, SortingOrder order)
        {
            Column = column ?? throw new ArgumentNullException(nameof(column));

            Order = order;
            DataGetter = column.DataGetterExpression;
        }

        ///<inheritdoc/>
        public IQueryable<TData> Apply(IQueryable<TData> data)
        {
            return Order == SortingOrder.Ascending
                ? data.OrderBy(DataGetter)
                : data.OrderByDescending(DataGetter);
        }
    }
}
