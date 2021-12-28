// ----------------------------------------------------------------------
// <copyright file="ITableDataRepository.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    /// <summary>
    /// Table data repository interface.
    /// </summary>
    public interface ITableDataRepository
    {
        /// <summary>
        /// Get all registered table data (Id, DataType) items.
        /// </summary>
        /// <returns>The registered (Id, DataType) items.</returns>
        Task<IEnumerable<(string Id, Type DataType)>> GetTableDataIdsAndTypesAsync();

        /// <summary>
        /// Get all registered table data Ids.
        /// </summary>
        /// <returns>The registered Ids.</returns>
        Task<IEnumerable<string>> GetTableDataIdsAsync();

        /// <summary>
        /// Get typed table data matching the given table id.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <param name="tableId">Table id to match.</param>
        /// <returns>The matching typed table data.</returns>
        Task<ITableData<TData>> GetTableDataAsync<TData>(string tableId);

        /// <summary>
        /// Get table data matching the given table id.
        /// </summary>
        /// <param name="tableId">Table id to match.</param>
        /// <returns>The matching table data.</returns>
        Task<ITableData> GetTableDataAsync(string tableId);

        /// <summary>
        /// Register the given table data.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <param name="data">The table data to register.</param>
        void Register<TData>(ITableData<TData> data);
    }
}
