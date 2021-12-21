using System;
using System.Threading.Tasks;

namespace SoloX.TableModel.Options.Impl
{
    public class QueryableTableDataOptions<TData, TQueryableTableData> : ATableDataOptions, IQueryableTableDataOptions<TData, TQueryableTableData> 
        where TQueryableTableData : ITableData<TData>
    {
        public Func<string, IServiceProvider, TQueryableTableData> Factory { get; set; }

        public QueryableTableDataOptions(string tableDataId) : base(tableDataId)
        {
        }

        public override Task<ITableData> CreateModelInstanceAsync(IServiceProvider serviceProvider)
        {
            var tableData = Factory(TableDataId, serviceProvider);
            return Task.FromResult<ITableData>(tableData);
        }
    }
}