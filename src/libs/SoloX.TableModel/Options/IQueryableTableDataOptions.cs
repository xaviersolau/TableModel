using System;

namespace SoloX.TableModel.Options
{
    public interface IQueryableTableDataOptions<TData, TQueryableTableData>
    where TQueryableTableData:ITableData<TData>
    {
        Func<string, IServiceProvider, TQueryableTableData> Factory { get; set; }
    }
}