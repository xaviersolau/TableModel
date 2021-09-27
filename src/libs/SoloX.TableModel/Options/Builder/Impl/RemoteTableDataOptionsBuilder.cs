using SoloX.TableModel.Options;
using System;
using SoloX.TableModel.Options.Builder;
using SoloX.TableModel.Options.Impl;

namespace SoloX.TableModel.Options.Builder.Impl
{
    public class RemoteTableDataOptionsBuilder<TData> : ATableDataOptionsBuilder, IRemoteTableDataOptionsBuilder<TData>
    {
        private readonly Action<IRemoteTableDataOptions<TData>> tableDataOptionsSetup;

        public RemoteTableDataOptionsBuilder(string tableDataId, Action<IRemoteTableDataOptions<TData>> tableDataOptionsSetup)
            : base(tableDataId)
        {
            this.tableDataOptionsSetup = tableDataOptionsSetup;
        }

        public override ATableDataOptions Build()
        {
            var opt = new RemoteTableDataOptions<TData>(TableDataId);

            tableDataOptionsSetup(opt);

            return opt;
        }
    }
}
