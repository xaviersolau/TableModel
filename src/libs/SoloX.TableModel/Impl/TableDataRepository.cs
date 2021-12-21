using Microsoft.Extensions.Options;
using SoloX.TableModel.Options.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    public class TableDataRepository : ITableDataRepository
    {
        private readonly IDictionary<string, ITableData> tableDataMap = new Dictionary<string, ITableData>();

        private readonly IDictionary<string, ATableDataOptions> tableDataOptions;
        private readonly IServiceProvider serviceProvider;

        public TableDataRepository(IOptions<TableModelOptions> options, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.tableDataOptions = options.Value.TableDataOptions.ToDictionary(x=>x.TableDataId);
        }

        public async Task<ITableData<TData>> GetTableDataAsync<TData>(string tableId)
        {
            return (ITableData<TData>) await GetTableDataAsync(tableId);
        }

        public async Task<ITableData> GetTableDataAsync(string tableId)
        {
            if (!this.tableDataMap.TryGetValue(tableId, out var tableData))
            {
                var tableDataOption = tableDataOptions[tableId];

                tableData = await tableDataOption.CreateModelInstanceAsync(this.serviceProvider);
                if (!tableData.DisableInstanceCaching)
                {
                    this.tableDataMap.Add(tableId, tableData);
                }
            }

            return tableData;
        }

        public Task<IEnumerable<string>> GetTableDataIdsAsync()
        {
            // TODO Add additional Id in case a user manually registered some.
            return Task.FromResult(this.tableDataOptions.Keys.AsEnumerable());
        }

        public void Register<TData>(ITableData<TData> data)
        {
            this.tableDataMap.Add(data.Id, data);
        }
    }
}
