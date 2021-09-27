using Microsoft.Extensions.Options;
using SoloX.TableModel.Options.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using SoloX.TableModel.Dto;
using SoloX.TableModel.Services;

namespace SoloX.TableModel.Impl
{
    public class TableStructureRepository : ITableStructureRepository
    {
        public readonly IDictionary<string, ITableStructure> tableStructures = new Dictionary<string, ITableStructure>();
        public readonly IDictionary<string, IDictionary<string, ITableDecorator>> tableDecorators = new Dictionary<string, IDictionary<string, ITableDecorator>>();

        private readonly IDictionary<string, ATableStructureOptions> tableStructureOptions;
        private readonly IServiceProvider serviceProvider;

        public TableStructureRepository(IOptions<TableModelOptions> options, IServiceProvider serviceProvider)
        {
            this.tableStructureOptions = options.Value.TableStructureOptions.ToDictionary(x => x.TableStructureId);
            this.serviceProvider = serviceProvider;
        }

        public async Task<ITableDecorator<TData, TDecorator>> GetTableDecoratorAsync<TData, TDecorator>(string tableId, string decoratorId)
        {
            var tableDecorator = await GetTableDecoratorAsync(tableId, decoratorId);

            return (ITableDecorator<TData, TDecorator>)tableDecorator;
        }

        public async Task<ITableDecorator> GetTableDecoratorAsync(string tableId, string decoratorId)
        {
            if (!(this.tableDecorators.TryGetValue(tableId, out var decorators) && decorators.TryGetValue(decoratorId, out var tableDecorator)))
            {
                var tableStructureOption = this.tableStructureOptions[tableId];

                var tableDecoratorOption = tableStructureOption.TableDecoratorOptions.FirstOrDefault(x => x.TableDecoratorId.Equals(decoratorId, StringComparison.Ordinal));

                tableDecorator = await tableDecoratorOption.CreateModelInstanceAsync(this.serviceProvider, this);

                if (decorators == null)
                {
                    decorators = new Dictionary<string, ITableDecorator>();

                    this.tableDecorators.Add(tableId, decorators);
                }

                decorators.Add(decoratorId, tableDecorator);
            }

            return (ITableDecorator)tableDecorator;
        }

        public async Task<ITableStructure<TData, TId>> GetTableStructureAsync<TData, TId>(string tableId)
        {
            var tableStructure = await GetTableStructureAsync(tableId);

            return (ITableStructure<TData, TId>)tableStructure;
        }

        public async Task<ITableStructure<TData>> GetTableStructureAsync<TData>(string tableId)
        {
            var tableStructure = await GetTableStructureAsync(tableId);

            return (ITableStructure<TData>)tableStructure;
        }

        public async Task<ITableStructure> GetTableStructureAsync(string tableId)
        {
            if (!this.tableStructures.TryGetValue(tableId, out var tableStructure))
            {
                var tableStructureOption = this.tableStructureOptions[tableId];

                tableStructure = await tableStructureOption.CreateModelInstanceAsync(this.serviceProvider);

                this.tableStructures.Add(tableId, tableStructure);
            }

            return tableStructure;
        }


        public Task<IEnumerable<string>> GetTableStructureIdsAsync()
        {
            // TODO Add additional Id in case a user manually registered some.
            return Task.FromResult(this.tableStructureOptions.Keys.AsEnumerable());
        }

        public void Register<TData, TId>(ITableStructure<TData, TId> table)
        {
            this.tableStructures.Add(table.Id, table);
        }

        public void Register<TData, TDecorator>(ITableDecorator<TData, TDecorator> decorator)
        {
            if (!this.tableDecorators.TryGetValue(decorator.TableStructure.Id, out var map))
            {
                map = new Dictionary<string, ITableDecorator>();
                this.tableDecorators.Add(decorator.TableStructure.Id, map);
            }

            map.Add(decorator.Id, decorator);
        }
    }
}
