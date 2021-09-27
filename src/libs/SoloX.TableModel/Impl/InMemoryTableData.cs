using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    public class InMemoryTableData<TData> : ATableData<TData>
    {
        private IEnumerable<TData> data;

        public InMemoryTableData(string id, IEnumerable<TData> data)
            : base(id)
        {
            this.data = data;
        }

        public override Task<IEnumerable<TData>> GetDataAsync()
        {
            return Task.FromResult(data);
        }

        public override Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter)
        {
            var processedData = data.AsQueryable();

            processedData = filter.Apply(processedData);

            processedData = sorting.Apply(processedData);

            return Task.FromResult<IEnumerable<TData>>(processedData);
        }

        public override Task<IEnumerable<TData>> GetDataAsync(ITableFilter<TData> filter)
        {
            var processedData = data.AsQueryable();

            return Task.FromResult<IEnumerable<TData>>(filter.Apply(processedData));
        }

        public override Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting)
        {
            var processedData = data.AsQueryable();

            return Task.FromResult<IEnumerable<TData>>(sorting.Apply(processedData));
        }

        public override Task<IEnumerable<TData>> GetDataPageAsync(int offset, int pageSize)
        {
            return Task.FromResult(data.Skip(offset).Take(pageSize));
        }

        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter, int offset, int pageSize)
        {
            var processedData = data.AsQueryable();

            processedData = filter.Apply(processedData);

            processedData = sorting.Apply(processedData);

            return Task.FromResult<IEnumerable<TData>>(processedData.Skip(offset).Take(pageSize));
        }

        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableFilter<TData> filter, int offset, int pageSize)
        {
            var processedData = data.AsQueryable();

            processedData = filter.Apply(processedData);

            return Task.FromResult<IEnumerable<TData>>(processedData.Skip(offset).Take(pageSize));
        }

        public override Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, int offset, int pageSize)
        {
            var processedData = data.AsQueryable();

            processedData = sorting.Apply(processedData);

            return Task.FromResult<IEnumerable<TData>>(processedData.Skip(offset).Take(pageSize));
        }

        public override Task<int> GetDataCountAsync()
        {
            return Task.FromResult(data.Count());
        }

        public override Task<int> GetDataCountAsync(ITableFilter<TData> filter)
        {
            var processedData = data.AsQueryable();

            processedData = filter.Apply(processedData);

            return Task.FromResult(processedData.Count());
        }
    }
}
