using System;
using SoloX.TableModel.Options.Impl;

namespace SoloX.TableModel.Options.Builder.Impl
{
    public class QueryableTableDataOptionsBuilder<TData, TQueryableTableData> : ATableDataOptionsBuilder, IQueryableTableDataOptionsBuilder<TData, TQueryableTableData>
        where TQueryableTableData : ITableData<TData>
    {
        private readonly Action<IQueryableTableDataOptions<TData, TQueryableTableData>> tableDataOptionsSetup;

        public QueryableTableDataOptionsBuilder(string tableDataId, Action<IQueryableTableDataOptions<TData, TQueryableTableData>> tableDataOptionsSetup) : base(tableDataId)
        {
            this.tableDataOptionsSetup = tableDataOptionsSetup;
        }

        public override ATableDataOptions Build()
        {
            var opt = new QueryableTableDataOptions<TData, TQueryableTableData>(TableDataId);

            tableDataOptionsSetup(opt);

            return opt;
        }
    }
}