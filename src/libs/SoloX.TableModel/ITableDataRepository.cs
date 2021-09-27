using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface ITableDataRepository
    {
        Task<IEnumerable<string>> GetTableDataIdsAsync();

        Task<ITableData<TData>> GetTableDataAsync<TData>(string tableId);
        Task<ITableData> GetTableDataAsync(string tableId);

        void Register<TData>(ITableData<TData> data);
    }
}
