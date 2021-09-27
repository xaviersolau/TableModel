using System.Collections.Generic;

namespace SoloX.TableModel.Options
{
    public interface IMemoryTableDataOptions<TData>
    {
        void AddData(IEnumerable<TData> data);
    }
}
