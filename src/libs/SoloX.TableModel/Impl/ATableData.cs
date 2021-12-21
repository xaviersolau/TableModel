using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    public abstract class ATableData<TData> : ITableData<TData>
    {
        protected ATableData(string id, bool disableInstanceCaching = false)
        {
            Id = id;
            DisableInstanceCaching = disableInstanceCaching;
        }

        public string Id { get; }
        public bool DisableInstanceCaching { get; }

        public void Accept(ITableDataVisitor visitor)
        {
            visitor.Visit(this);
        }

        public TResult Accept<TResult>(ITableDataVisitor<TResult> visitor)
        {
            return visitor.Visit(this);
        }

        public TResult Accept<TResult, TArg>(ITableDataVisitor<TResult, TArg> visitor, TArg arg)
        {
            return visitor.Visit(this, arg);
        }

        public abstract Task<IEnumerable<TData>> GetDataAsync();

        public abstract Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter);

        public abstract Task<IEnumerable<TData>> GetDataAsync(ITableFilter<TData> filter);

        public abstract Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting);

        public abstract Task<IEnumerable<TData>> GetDataPageAsync(int offset, int pageSize);

        public abstract Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter, int offset, int pageSize);

        public abstract Task<IEnumerable<TData>> GetDataPageAsync(ITableFilter<TData> filter, int offset, int pageSize);

        public abstract Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, int offset, int pageSize);

        public abstract Task<int> GetDataCountAsync();

        public abstract Task<int> GetDataCountAsync(ITableFilter<TData> filter);

    }
}
