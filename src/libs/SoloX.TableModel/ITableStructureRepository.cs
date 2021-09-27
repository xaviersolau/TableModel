using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface ITableStructureRepository
    {
        Task<IEnumerable<string>> GetTableStructureIdsAsync();

        Task<ITableStructure<TData, TId>> GetTableStructureAsync<TData, TId>(string tableId);
        Task<ITableStructure<TData>> GetTableStructureAsync<TData>(string tableId);
        Task<ITableStructure> GetTableStructureAsync(string tableId);

        Task<ITableDecorator<TData, TDecorator>> GetTableDecoratorAsync<TData, TDecorator>(string tableId, string decoratorId);
        Task<ITableDecorator> GetTableDecoratorAsync(string tableId, string decoratorId);

        void Register<TData, TId>(ITableStructure<TData, TId> table);

        void Register<TData, TDecorator>(ITableDecorator<TData, TDecorator> decorator);

    }
}
