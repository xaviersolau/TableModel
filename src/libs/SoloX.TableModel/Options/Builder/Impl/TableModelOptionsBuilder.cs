// ----------------------------------------------------------------------
// <copyright file="TableModelOptionsBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using SoloX.TableModel.Options.Impl;

namespace SoloX.TableModel.Options.Builder.Impl
{
    /// <summary>
    /// Table model option builder.
    /// </summary>
    public class TableModelOptionsBuilder : ITableModelOptionsBuilder
    {
        private readonly Action<TableModelOptionsBuilder> setupAction;

        private readonly List<ATableStructureOptionsBuilder> tableStructureOptionsBuilders = new List<ATableStructureOptionsBuilder>();
        private readonly List<ATableDataOptionsBuilder> tableDataOptionsBuilders = new List<ATableDataOptionsBuilder>();

        /// <summary>
        /// Setup Table model option builder.
        /// </summary>
        /// <param name="setupAction">Setup delegate.</param>
        public TableModelOptionsBuilder(Action<TableModelOptionsBuilder> setupAction)
        {
            this.setupAction = setupAction;
        }

        /// <inheritdoc/>
        public ILocalTableStructureOptionsBuilder<TData, TId> UseTableStructure<TData, TId>(string tableId, Action<ILocalTableStructureOptions<TData, TId>> configAction)
        {
            var instanceBuilder = new LocalTableStructureOptionsBuilder<TData, TId>(tableId, configAction);

            this.tableStructureOptionsBuilders.Add(instanceBuilder);

            return instanceBuilder;
        }

        /// <inheritdoc/>
        public ILocalTableStructureOptionsBuilder<TData, TId> UseTableStructure<TData, TId>(Action<ILocalTableStructureOptions<TData, TId>> configAction)
        {
            return UseTableStructure<TData, TId>(typeof(TData).FullName, configAction);
        }

        /// <inheritdoc/>
        public IRemoteTableStructureOptionsBuilder<TData, TId> UseRemoteTableStructure<TData, TId>(string tableId, Action<IRemoteTableStructureOptions<TData, TId>> configAction)
        {
            var instanceBuilder = new RemoteTableStructureOptionsBuilder<TData, TId>(tableId, configAction);

            this.tableStructureOptionsBuilders.Add(instanceBuilder);

            return instanceBuilder;
        }

        /// <inheritdoc/>
        public IRemoteTableStructureOptionsBuilder<TData, TId> UseRemoteTableStructure<TData, TId>(Action<IRemoteTableStructureOptions<TData, TId>> configAction)
        {
            return UseRemoteTableStructure<TData, TId>(typeof(TData).FullName, configAction);
        }

        /// <inheritdoc/>
        public IMemoryTableDataOptionsBuilder<TData> UseMemoryTableData<TData>(string tableId, Action<IMemoryTableDataOptions<TData>> configAction)
        {
            var instanceBuilder = new MemoryTableDataOptionsBuilder<TData>(tableId, configAction);

            this.tableDataOptionsBuilders.Add(instanceBuilder);

            return instanceBuilder;
        }

        /// <inheritdoc/>
        public IMemoryTableDataOptionsBuilder<TData> UseMemoryTableData<TData>(Action<IMemoryTableDataOptions<TData>> configAction)
        {
            return UseMemoryTableData(typeof(TData).FullName, configAction);
        }

        /// <inheritdoc/>
        public IRemoteTableDataOptionsBuilder<TData> UseRemoteTableData<TData>(string tableId, Action<IRemoteTableDataOptions<TData>> configAction)
        {
            var instanceBuilder = new RemoteTableDataOptionsBuilder<TData>(tableId, configAction);

            this.tableDataOptionsBuilders.Add(instanceBuilder);

            return instanceBuilder;
        }

        /// <inheritdoc/>
        public IRemoteTableDataOptionsBuilder<TData> UseRemoteTableData<TData>(Action<IRemoteTableDataOptions<TData>> configAction)
        {
            return UseRemoteTableData(typeof(TData).FullName, configAction);
        }

        /// <inheritdoc/>
        public IQueryableTableDataOptionsBuilder<TData, TQueryableTableData> UseQueryableTableData<TData, TQueryableTableData>(
            string tableId,
            Action<IQueryableTableDataOptions<TData, TQueryableTableData>> configAction)
            where TQueryableTableData : ITableData<TData>
        {
            var instanceBuilder = new QueryableTableDataOptionsBuilder<TData, TQueryableTableData>(tableId, configAction);

            this.tableDataOptionsBuilders.Add(instanceBuilder);

            return instanceBuilder;
        }

        /// <inheritdoc/>
        public IQueryableTableDataOptionsBuilder<TData, TQueryableTableData> UseQueryableTableData<TData, TQueryableTableData>(Action<IQueryableTableDataOptions<TData, TQueryableTableData>> configAction) where TQueryableTableData : ITableData<TData>
        {
            return UseQueryableTableData(typeof(TData).FullName, configAction);
        }

        internal void Build(TableModelOptions options)
        {
            this.setupAction(this);

            var optionsList = new List<ATableStructureOptions>();

            foreach (var tableStructureOptionsBuilder in this.tableStructureOptionsBuilders)
            {
                optionsList.Add(tableStructureOptionsBuilder.Build());
            }

            options.TableStructureOptions = optionsList;

            var dataList = new List<ATableDataOptions>();

            foreach (var tableDataOptionsBuilder in this.tableDataOptionsBuilders)
            {
                dataList.Add(tableDataOptionsBuilder.Build());
            }

            options.TableDataOptions = dataList;
        }
    }
}
