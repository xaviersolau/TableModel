using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Options.Builder
{
    public interface ITableModelOptionsBuilder
    {
        ILocalTableStructureOptionsBuilder<TData, TId> UseTableStructure<TData, TId>(string tableId, Action<ILocalTableStructureOptions<TData, TId>> configAction);

        IRemoteTableStructureOptionsBuilder<TData, TId> UseRemoteTableStructure<TData, TId>(string tableId, Action<IRemoteTableStructureOptions<TData, TId>> configAction);

        IMemoryTableDataOptionsBuilder<TData> UseMemoryTableData<TData>(string tableId, Action<IMemoryTableDataOptions<TData>> configAction);

        IRemoteTableDataOptionsBuilder<TData> UseRemoteTableData<TData>(string tableId, Action<IRemoteTableDataOptions<TData>> configAction);
    }
}
