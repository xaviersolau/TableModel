using SoloX.TableModel.Options;
using System;
using SoloX.TableModel.Options.Builder;
using SoloX.TableModel.Options.Impl;

namespace SoloX.TableModel.Options.Builder.Impl
{
    public class MemoryTableDataOptionsBuilder<TData> : ATableDataOptionsBuilder, IMemoryTableDataOptionsBuilder<TData>
    {
        private readonly Action<IMemoryTableDataOptions<TData>> tableDataOptionsSetup;

        public MemoryTableDataOptionsBuilder(string tableDataId, Action<IMemoryTableDataOptions<TData>> tableDataOptionsSetup)
            : base(tableDataId)
        {
            this.tableDataOptionsSetup = tableDataOptionsSetup;
        }

        public override ATableDataOptions Build()
        {
            var opt = new MemoryTableDataOptions<TData>(TableDataId);

            tableDataOptionsSetup(opt);

            return opt;
        }
    }
}
