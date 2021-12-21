using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    public abstract class AQueryableTableData<TData> : ATableData<TData>
    {
        protected AQueryableTableData(string id)
            : base(id, true)
        {
        }

        public override Task<IEnumerable<TData>> GetDataAsync()
        {
            var request = QueryData();

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        public override Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter)
        {
            var request = QueryData();

            request = filter.Apply(request);

            request = sorting.Apply(request);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        public override Task<IEnumerable<TData>> GetDataAsync(ITableFilter<TData> filter)
        {
            var request = QueryData();

            request = filter.Apply(request);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        public override Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting)
        {
            var request = QueryData();

            request = sorting.Apply(request);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        public override Task<IEnumerable<TData>> GetDataPageAsync(int offset, int pageSize)
        {
            var request = QueryData();

            request = request.Skip(offset).Take(pageSize);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter, int offset, int pageSize)
        {
            var request = QueryData();

            request = filter.Apply(request);

            request = sorting.Apply(request);

            request = request.Skip(offset).Take(pageSize);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableFilter<TData> filter, int offset, int pageSize)
        {
            var request = QueryData();

            request = filter.Apply(request);

            request = request.Skip(offset).Take(pageSize);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, int offset, int pageSize)
        {
            var request = QueryData();

            request = sorting.Apply(request);

            request = request.Skip(offset).Take(pageSize);

            return Task.FromResult<IEnumerable<TData>>(request);
        }

        public override Task<int> GetDataCountAsync()
        {
            var request = QueryData();

            return Task.FromResult(request.Count());
        }

        public override Task<int> GetDataCountAsync(ITableFilter<TData> filter)
        {
            var request = QueryData();

            request = filter.Apply(request);

            return Task.FromResult(request.Count());
        }

        protected abstract IQueryable<TData> QueryData();
    }
}
