﻿// ----------------------------------------------------------------------
// <copyright file="TableDataRepository.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using SoloX.TableModel.Options.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if NETSTANDARD2_1
using ArgumentNullException = SoloX.TableModel.Utils.ArgumentNullException;
#endif

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// Table data repository.
    /// </summary>
    public class TableDataRepository : ITableDataRepository
    {
        private readonly Dictionary<string, ITableData> tableDataMap = new Dictionary<string, ITableData>();

        private readonly IDictionary<string, ATableDataOptions> tableDataOptions;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Setup a TableDataRepository with the provided options.
        /// </summary>
        /// <param name="options">Table model options.</param>
        /// <param name="serviceProvider">Dependency injection service provider.</param>
        public TableDataRepository(IOptions<TableModelOptions> options, IServiceProvider serviceProvider)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));
            ArgumentNullException.ThrowIfNull(serviceProvider, nameof(serviceProvider));

            this.serviceProvider = serviceProvider;
            this.tableDataOptions = options.Value.TableDataOptions.ToDictionary(x => x.TableDataId);
        }

        ///<inheritdoc/>
        public async Task<ITableData<TData>> GetTableDataAsync<TData>(string tableId)
        {
            return (ITableData<TData>)await GetTableDataAsync(tableId).ConfigureAwait(false);
        }

        ///<inheritdoc/>
        public async Task<ITableData> GetTableDataAsync(string tableId)
        {
            if (!this.tableDataMap.TryGetValue(tableId, out var tableData))
            {
                var tableDataOption = this.tableDataOptions[tableId];

                tableData = await tableDataOption.CreateModelInstanceAsync(this.serviceProvider).ConfigureAwait(false);
                if (!tableData.DisableInstanceCaching)
                {
                    this.tableDataMap.Add(tableId, tableData);
                }
            }

            return tableData;
        }

        ///<inheritdoc/>
        public Task<ITableData<TData>> GetTableDataAsync<TData>()
        {
            return GetTableDataAsync<TData>(typeof(TData).FullName);
        }

        ///<inheritdoc/>
        public Task<IEnumerable<string>> GetTableDataIdsAsync()
        {
            // TODO Add additional Id in case a user manually registered some.
            return Task.FromResult(this.tableDataOptions.Keys.AsEnumerable());
        }

        ///<inheritdoc/>
        public Task<IEnumerable<(string Id, Type DataType)>> GetTableDataIdsAndTypesAsync()
        {
            // TODO Add additional Id in case a user manually registered some.
            return Task.FromResult(this.tableDataOptions.Select(i => (i.Key, i.Value.DataType)).AsEnumerable());
        }

        ///<inheritdoc/>
        public void Register<TData>(ITableData<TData> data)
        {
            ArgumentNullException.ThrowIfNull(data, nameof(data));

            this.tableDataMap.Add(data.Id, data);
        }
    }
}
