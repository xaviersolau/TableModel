// ----------------------------------------------------------------------
// <copyright file="ColumnFilter.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.ExpressionTools.Transform.Impl;
using System;
using System.Linq.Expressions;

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// Column filter descriptor.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TColumn">Column data type.</typeparam>
    public class ColumnFilter<TData, TColumn> : DataFilterBase<TData>, IColumnFilter<TData, TColumn>
    {
        ///<inheritdoc/>
        IColumn<TData> IColumnFilter<TData>.Column => Column;

        ///<inheritdoc/>
        public IColumn<TData, TColumn> Column { get; }

        ///<inheritdoc/>
        public Expression<Func<TColumn, bool>> Filter { get; }

        /// <summary>
        /// Setup a ColumnFilter.
        /// </summary>
        /// <param name="column">Column to apply the filter on.</param>
        /// <param name="filter">Filter expression.</param>
        public ColumnFilter(IColumn<TData, TColumn> column, Expression<Func<TColumn, bool>> filter)
            : base(BuildDataFilter(column, filter, out var inLinedFilter), false)
        {
            Column = column;
            Filter = inLinedFilter;
        }

        ///<inheritdoc/>
        public void Accept(IColumnFilterVisitor<TData> visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }

        ///<inheritdoc/>
        public TResult Accept<TResult>(IColumnFilterVisitor<TData, TResult> visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            return visitor.Visit(this);
        }

        ///<inheritdoc/>
        public TResult Accept<TResult, TArg>(IColumnFilterVisitor<TData, TResult, TArg> visitor, TArg arg)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            return visitor.Visit(this, arg);
        }

        private static Expression<Func<TData, bool>> BuildDataFilter(
            IColumn<TData, TColumn> column,
            Expression<Func<TColumn, bool>> filter,
            out Expression<Func<TColumn, bool>> inLinedFilter)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            // Value filter : (v) => 5 > v > 10
            // Column value : (d) => d.v
            // Resulting filter expression :
            // (d) => 5 > d.v > 10

            var constInliner = new ConstantInliner();
            inLinedFilter = constInliner.Amend(filter);

            var inliner = new SingleParameterInliner();
            return inliner.Amend(column.DataGetterExpression, inLinedFilter);
        }
    }
}
