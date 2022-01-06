// ----------------------------------------------------------------------
// <copyright file="TableStructureRepository.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.Options;
using SoloX.TableModel.Options.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// Table structure repository.
    /// </summary>
    public class TableStructureRepository : ITableStructureRepository
    {
        private readonly IDictionary<string, ITableStructure> tableStructures = new Dictionary<string, ITableStructure>();
        private readonly IDictionary<string, IDictionary<string, ITableDecorator>> tableDecorators = new Dictionary<string, IDictionary<string, ITableDecorator>>();

        private readonly IDictionary<string, ATableStructureOptions> tableStructureOptions;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Setup a TableStructureRepository.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="serviceProvider"></param>
        public TableStructureRepository(IOptions<TableModelOptions> options, IServiceProvider serviceProvider)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            this.tableStructureOptions = options.Value.TableStructureOptions.ToDictionary(x => x.TableStructureId);
        }

        ///<inheritdoc/>
        public async Task<ITableDecorator<TData, TDecorator>> GetTableDecoratorAsync<TData, TDecorator>(string tableId, string decoratorId)
        {
            var tableDecorator = await GetTableDecoratorAsync(tableId, decoratorId).ConfigureAwait(false);

            return (ITableDecorator<TData, TDecorator>)tableDecorator;
        }

        ///<inheritdoc/>
        public Task<ITableDecorator<TData, TDecorator>> GetTableDecoratorAsync<TData, TDecorator>(string decoratorId)
        {
            return GetTableDecoratorAsync<TData, TDecorator>(typeof(TData).FullName, decoratorId);
        }

        ///<inheritdoc/>
        public async Task<ITableDecorator> GetTableDecoratorAsync(string tableId, string decoratorId)
        {
            if (!(this.tableDecorators.TryGetValue(tableId, out var decorators) && decorators.TryGetValue(decoratorId, out var tableDecorator)))
            {
                var tableStructureOption = this.tableStructureOptions[tableId];

                var tableDecoratorOption = tableStructureOption.TableDecoratorOptions.FirstOrDefault(x => x.TableDecoratorId.Equals(decoratorId, StringComparison.Ordinal));

                tableDecorator = await tableDecoratorOption.CreateModelInstanceAsync(this.serviceProvider, this).ConfigureAwait(false);

                if (decorators == null)
                {
                    decorators = new Dictionary<string, ITableDecorator>();

                    this.tableDecorators.Add(tableId, decorators);
                }

                decorators.Add(decoratorId, tableDecorator);
            }

            return tableDecorator;
        }

        ///<inheritdoc/>
        public async Task<ITableStructure<TData, TId>> GetTableStructureAsync<TData, TId>(string tableId)
        {
            var tableStructure = await GetTableStructureAsync(tableId).ConfigureAwait(false);

            return (ITableStructure<TData, TId>)tableStructure;
        }

        ///<inheritdoc/>
        public Task<ITableStructure<TData, TId>> GetTableStructureAsync<TData, TId>()
        {
            return GetTableStructureAsync<TData, TId>(typeof(TData).FullName);
        }

        ///<inheritdoc/>
        public async Task<ITableStructure<TData>> GetTableStructureAsync<TData>(string tableId)
        {
            var tableStructure = await GetTableStructureAsync(tableId).ConfigureAwait(false);

            return (ITableStructure<TData>)tableStructure;
        }

        ///<inheritdoc/>
        public Task<ITableStructure<TData>> GetTableStructureAsync<TData>()
        {
            return GetTableStructureAsync<TData>(typeof(TData).FullName);
        }

        ///<inheritdoc/>
        public async Task<ITableStructure> GetTableStructureAsync(string tableId)
        {
            if (!this.tableStructures.TryGetValue(tableId, out var tableStructure))
            {
                var tableStructureOption = this.tableStructureOptions[tableId];

                tableStructure = await tableStructureOption.CreateModelInstanceAsync(this.serviceProvider).ConfigureAwait(false);

                this.tableStructures.Add(tableId, tableStructure);
            }

            return tableStructure;
        }

        ///<inheritdoc/>
        public Task<IEnumerable<string>> GetTableStructureIdsAsync()
        {
            // TODO Add additional Id in case a user manually registered some.
            return Task.FromResult(this.tableStructureOptions.Keys.AsEnumerable());
        }

        ///<inheritdoc/>
        public void Register<TData, TId>(ITableStructure<TData, TId> table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            this.tableStructures.Add(table.Id, table);
        }

        ///<inheritdoc/>
        public void Register<TData, TDecorator>(ITableDecorator<TData, TDecorator> decorator)
        {
            if (decorator == null)
            {
                throw new ArgumentNullException(nameof(decorator));
            }

            if (!this.tableDecorators.TryGetValue(decorator.TableStructure.Id, out var map))
            {
                map = new Dictionary<string, ITableDecorator>();
                this.tableDecorators.Add(decorator.TableStructure.Id, map);
            }

            map.Add(decorator.Id, decorator);
        }
    }
}
