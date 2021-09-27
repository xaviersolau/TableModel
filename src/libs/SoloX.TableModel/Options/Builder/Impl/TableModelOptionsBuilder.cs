using SoloX.TableModel.Options;
using System;
using System.Collections.Generic;
using SoloX.TableModel.Options.Builder;
using SoloX.TableModel.Options.Impl;

namespace SoloX.TableModel.Options.Builder.Impl
{
    public class TableModelOptionsBuilder : ITableModelOptionsBuilder
    {
        private readonly Action<TableModelOptionsBuilder> setupAction;

        private readonly List<ATableStructureOptionsBuilder> tableStructureOptionsBuilders = new List<ATableStructureOptionsBuilder>();
        private readonly List<ATableDataOptionsBuilder> tableDataOptionsBuilders = new List<ATableDataOptionsBuilder>();

        public TableModelOptionsBuilder(Action<TableModelOptionsBuilder> setupAction)
        {
            this.setupAction = setupAction;
        }

        public ILocalTableStructureOptionsBuilder<TData, TId> UseTableStructure<TData, TId>(string tableId, Action<ILocalTableStructureOptions<TData, TId>> configAction)
        {
            var instanceBuilder = new LocalTableStructureOptionsBuilder<TData, TId>(tableId, configAction);

            this.tableStructureOptionsBuilders.Add(instanceBuilder);

            return instanceBuilder;
        }

        public IMemoryTableDataOptionsBuilder<TData> UseMemoryTableData<TData>(string tableId, Action<IMemoryTableDataOptions<TData>> configAction)
        {
            var instanceBuilder = new MemoryTableDataOptionsBuilder<TData>(tableId, configAction);

            this.tableDataOptionsBuilders.Add(instanceBuilder);

            return instanceBuilder;
        }

        public IRemoteTableStructureOptionsBuilder<TData, TId> UseRemoteTableStructure<TData, TId>(string tableId, Action<IRemoteTableStructureOptions<TData, TId>> configAction)
        {
            var instanceBuilder = new RemoteTableStructureOptionsBuilder<TData, TId>(tableId, configAction);

            this.tableStructureOptionsBuilders.Add(instanceBuilder);

            return instanceBuilder;
        }

        public IRemoteTableDataOptionsBuilder<TData> UseRemoteTableData<TData>(string tableId, Action<IRemoteTableDataOptions<TData>> configAction)
        {
            var instanceBuilder = new RemoteTableDataOptionsBuilder<TData>(tableId, configAction);

            this.tableDataOptionsBuilders.Add(instanceBuilder);

            return instanceBuilder;
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
