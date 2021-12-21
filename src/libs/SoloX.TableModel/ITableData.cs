using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface ITableData
    {
        string Id { get; }
        bool DisableInstanceCaching { get; }

        void Accept(ITableDataVisitor visitor);
        TResult Accept<TResult>(ITableDataVisitor<TResult> visitor);
        TResult Accept<TResult, TArg>(ITableDataVisitor<TResult, TArg> visitor, TArg arg);

    }

    public interface ITableData<TData> : ITableData
    {
        Task<IEnumerable<TData>> GetDataPageAsync(int offset, int pageSize);

        Task<IEnumerable<TData>> GetDataAsync();

        Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter, int offset, int pageSize);

        Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter);

        Task<IEnumerable<TData>> GetDataPageAsync(ITableFilter<TData> filter, int offset, int pageSize);

        Task<IEnumerable<TData>> GetDataAsync(ITableFilter<TData> filter);

        Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, int offset, int pageSize);

        Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting);

        Task<int> GetDataCountAsync();

        Task<int> GetDataCountAsync(ITableFilter<TData> filter);
    }
}
