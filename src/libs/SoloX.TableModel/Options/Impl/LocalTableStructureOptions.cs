// ----------------------------------------------------------------------
// <copyright file="LocalTableStructureOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.TableModel.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SoloX.TableModel.Options.Impl
{
    /// <summary>
    /// Local table structure options.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TId">Table Id type.</typeparam>
    public class LocalTableStructureOptions<TData, TId> : ATableStructureOptions, ILocalTableStructureOptions<TData, TId>, ILocalTableStructureDataOptions<TData, TId>
    {
        private readonly List<IColumn<TData>> dataColumns = new List<IColumn<TData>>();

        /// <summary>
        /// Get Column Id.
        /// </summary>
        public IColumn<TData, TId> IdColumn { get; private set; }

        /// <summary>
        /// Get table data columns.
        /// </summary>
        public IEnumerable<IColumn<TData>> DataColumns => this.dataColumns;

        /// <summary>
        /// Setup LocalTableStructureOptions instance with Id and decorator options.
        /// </summary>
        /// <param name="tableStructureId">Table structure Id.</param>
        /// <param name="tableDecoratorOptions">Decorator options.</param>
        public LocalTableStructureOptions(string tableStructureId, IEnumerable<ATableDecoratorOptions> tableDecoratorOptions)
            : base(tableStructureId, tableDecoratorOptions)
        { }

        /// <inheritdoc/>
        public ILocalTableStructureDataOptions<TData, TId> AddIdColumn(string columnId, Expression<Func<TData, TId>> idGetterExpression, bool canSort, bool canFilter)
        {
            if (IdColumn != null)
            {
                throw new InvalidDataException("Id column already defined");
            }

            IdColumn = new Column<TData, TId>(columnId, idGetterExpression, canSort, canFilter);

            return this;
        }

        /// <inheritdoc/>
        public ILocalTableStructureDataOptions<TData, TId> AddColumn<TColumn>(string columnId, Expression<Func<TData, TColumn>> dataGetterExpression, bool canSort, bool canFilter)
        {
            this.dataColumns.Add(new Column<TData, TColumn>(columnId, dataGetterExpression, canSort, canFilter));

            return this;
        }

        /// <inheritdoc/>
        public override Task<ITableStructure> CreateModelInstanceAsync(IServiceProvider serviceProvider)
        {
            var tableStructure = new TableStructure<TData, TId>(TableStructureId, IdColumn, DataColumns);

            return Task.FromResult<ITableStructure>(tableStructure);
        }
    }
}
