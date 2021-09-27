using System;
using SoloX.TableModel.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoloX.TableModel.Impl;

namespace SoloX.TableModel.Options.Impl
{
    public class MemoryTableDataOptions<TData> : ATableDataOptions, IMemoryTableDataOptions<TData>
    {
        private IEnumerable<TData> data;

        public MemoryTableDataOptions(string tableId)
            : base(tableId)
        {
        }

        public void AddData(IEnumerable<TData> data)
        {
            this.data = data;
        }

        public override Task<ITableData> CreateModelInstanceAsync(IServiceProvider serviceProvider)
        {
            var tableData = new InMemoryTableData<TData>(TableDataId, this.data);
            return Task.FromResult<ITableData>(tableData);
        }
    }
}
