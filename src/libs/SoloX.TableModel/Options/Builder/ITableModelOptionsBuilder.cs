using System;
using SoloX.TableModel.Options.Impl;

namespace SoloX.TableModel.Options.Builder
{
    public interface ITableModelOptionsBuilder
    {
        ILocalTableStructureOptionsBuilder<TData, TId> UseTableStructure<TData, TId>(string tableId, Action<ILocalTableStructureOptions<TData, TId>> configAction);

        IRemoteTableStructureOptionsBuilder<TData, TId> UseRemoteTableStructure<TData, TId>(string tableId, Action<IRemoteTableStructureOptions<TData, TId>> configAction);

        IMemoryTableDataOptionsBuilder<TData> UseMemoryTableData<TData>(string tableId, Action<IMemoryTableDataOptions<TData>> configAction);

        IRemoteTableDataOptionsBuilder<TData> UseRemoteTableData<TData>(string tableId, Action<IRemoteTableDataOptions<TData>> configAction);

        IQueryableTableDataOptionsBuilder<TData, TQueryableTableData> UseQueryableTableData<TData,TQueryableTableData>(string tableId, Action<IQueryableTableDataOptions<TData, TQueryableTableData>> configAction)
            where TQueryableTableData : ITableData<TData>;
    }
}
