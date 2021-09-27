using System;
using System.Threading.Tasks;

namespace SoloX.TableModel.Options.Impl
{
    public abstract class ATableDataOptions
    {
        protected ATableDataOptions(string tableDataId)
        {
            TableDataId = tableDataId;
        }

        public string TableDataId { get; }

        public abstract Task<ITableData> CreateModelInstanceAsync(IServiceProvider serviceProvider);
    }
}
