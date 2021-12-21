﻿// ----------------------------------------------------------------------
// <copyright file="Column.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq.Expressions;

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// Table data column descriptor.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TColumn">Column data type.</typeparam>
    public class Column<TData, TColumn> : IColumn<TData, TColumn>
    {
        private readonly Func<TData, TColumn> dataGetter;

        /// <summary>
        /// Setup a Column.
        /// </summary>
        /// <param name="id">Column Id.</param>
        /// <param name="dataGetterExpression">Column data getter expression (From a table data item).</param>
        /// <param name="canSort">Tells if the column can be sorted.</param>
        /// <param name="canFilter">Tells if the column can be filtered.</param>
        public Column(string id, Expression<Func<TData, TColumn>> dataGetterExpression, bool canSort = true, bool canFilter = true)
        {
            Id = id;
            CanSort = canSort;
            CanFilter = canFilter;
            DataGetterExpression = dataGetterExpression ?? throw new ArgumentNullException(nameof(dataGetterExpression));

            this.dataGetter = dataGetterExpression.Compile();
        }

        ///<inheritdoc/>
        public Type DataType => typeof(TColumn);

        ///<inheritdoc/>
        public string Id { get; private set; }

        ///<inheritdoc/>
        public bool CanSort { get; private set; }

        ///<inheritdoc/>
        public bool CanFilter { get; private set; }

        ///<inheritdoc/>
        public Expression<Func<TData, TColumn>> DataGetterExpression { get; }

        ///<inheritdoc/>
        public object GetObject(TData data)
        {
            return this.dataGetter(data);
        }

        ///<inheritdoc/>
        public TColumn GetValue(TData data)
        {
            return this.dataGetter(data);
        }

        ///<inheritdoc/>
        public void Accept(IColumnVisitor<TData> visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }

        ///<inheritdoc/>
        public TResult Accept<TResult>(IColumnVisitor<TData, TResult> visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            return visitor.Visit(this);
        }

        ///<inheritdoc/>
        public TResult Accept<TResult, TArg>(IColumnVisitor<TData, TResult, TArg> visitor, TArg arg)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            return visitor.Visit(this, arg);
        }
    }
}
