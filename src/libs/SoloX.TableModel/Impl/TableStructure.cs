﻿// ----------------------------------------------------------------------
// <copyright file="TableStructure.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

#if NETSTANDARD2_1
using ArgumentNullException = SoloX.TableModel.Utils.ArgumentNullException;
#else
using System;
#endif

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// Table structure descriptor.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TId">Table Id type.</typeparam>
    public class TableStructure<TData, TId> : ITableStructure<TData, TId>
    {
        private readonly Dictionary<string, IColumn<TData>> columnMap;

        /// <summary>
        /// Setup a TableStructure
        /// </summary>
        /// <param name="id">Table structure Id.</param>
        /// <param name="idColumn">Column providing Id from a table data item.</param>
        /// <param name="dataColumns">Data columns.</param>
        public TableStructure(string id, IColumn<TData, TId> idColumn, params IColumn<TData>[] dataColumns)
        : this(id, idColumn, dataColumns.AsEnumerable())
        {
        }

        /// <summary>
        /// Setup a TableStructure
        /// </summary>
        /// <param name="id">Table structure Id.</param>
        /// <param name="idColumn">Column providing Id from a table data item.</param>
        /// <param name="dataColumns">Data columns.</param>
        public TableStructure(string id, IColumn<TData, TId> idColumn, IEnumerable<IColumn<TData>> dataColumns)
        {
            ArgumentNullException.ThrowIfNull(idColumn, nameof(idColumn));
            ArgumentNullException.ThrowIfNull(dataColumns, nameof(dataColumns));

            Id = id;

            var columnList = new List<IColumn<TData>>();
            columnList.Add(idColumn);
            columnList.AddRange(dataColumns);

            Columns = columnList;
            IdColumn = idColumn;
            DataColumns = dataColumns;

            this.columnMap = new Dictionary<string, IColumn<TData>>();

            foreach (var col in columnList.Where(c => !string.IsNullOrEmpty(c.Id)))
            {
                this.columnMap.Add(col.Id, col);
            }
        }

        ///<inheritdoc/>
        public string Id { get; }

        ///<inheritdoc/>
        public IColumn<TData> this[string id] => this.columnMap[id];

        ///<inheritdoc/>
        public IEnumerable<IColumn<TData>> Columns { get; private set; }

        ///<inheritdoc/>
        public IEnumerable<IColumn<TData>> DataColumns { get; private set; }

        ///<inheritdoc/>
        public IColumn<TData, TId> IdColumn { get; private set; }

        ///<inheritdoc/>
        public void Accept(ITableStructureVisitor visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));

            visitor.Visit(this);
        }

        ///<inheritdoc/>
        public TResult Accept<TResult>(ITableStructureVisitor<TResult> visitor)
        {
            ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));

            return visitor.Visit(this);
        }

        ///<inheritdoc/>
        public TResult Accept<TResult, TArg>(ITableStructureVisitor<TResult, TArg> visitor, TArg arg)
        {
            ArgumentNullException.ThrowIfNull(visitor, nameof(visitor));

            return visitor.Visit(this, arg);
        }
    }
}
