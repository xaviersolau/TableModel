// ----------------------------------------------------------------------
// <copyright file="ITableData.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    /// <summary>
    /// Table data base interface providing id and visitor pattern support.
    /// </summary>
    public interface ITableData
    {
        /// <summary>
        /// Get the table data Id.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Get Disable Instance Caching to avoid reuse of table data instance.
        /// </summary>
        bool DisableInstanceCaching { get; }

        /// <summary>
        /// Accept the given visitor and visit the actual typed table data.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Accept(ITableDataVisitor visitor);

        /// <summary>
        /// Accept the given visitor and visit the actual typed table data to return the typed result.
        /// </summary>
        /// <typeparam name="TResult">The resulting type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        TResult Accept<TResult>(ITableDataVisitor<TResult> visitor);

        /// <summary>
        /// Accept the given visitor and visit the actual typed table data to return the typed result.
        /// </summary>
        /// <typeparam name="TResult">The resulting type.</typeparam>
        /// <typeparam name="TArg">The typed argument</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <param name="arg">The argument to transmit to the visitor.</param>
        TResult Accept<TResult, TArg>(ITableDataVisitor<TResult, TArg> visitor, TArg arg);
    }

    /// <summary>
    /// Typed table data interface.
    /// </summary>
    /// <typeparam name="TData">Data type.</typeparam>
    public interface ITableData<TData> : ITableData
    {
        /// <summary>
        /// Get a data page from the table data.
        /// </summary>
        /// <param name="offset">The offset of the page.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The page data.</returns>
        Task<IEnumerable<TData>> GetDataPageAsync(int offset, int pageSize);

        /// <summary>
        /// Get all data from the table data.
        /// </summary>
        /// <returns>All table data.</returns>
        Task<IEnumerable<TData>> GetDataAsync();

        /// <summary>
        /// Get a data page from the sorted and filtered table data.
        /// </summary>
        /// <param name="sorting">The sorting to apply.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <param name="offset">The page offset.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The page data.</returns>
        Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter, int offset, int pageSize);

        /// <summary>
        /// Get the sorted and filtered table data.
        /// </summary>
        /// <param name="sorting">The sorting to apply.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>The sorted and filtered table data.</returns>
        Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting, ITableFilter<TData> filter);

        /// <summary>
        /// Get a data page from the filtered table data.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <param name="offset">The page offset.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The page data.</returns>
        Task<IEnumerable<TData>> GetDataPageAsync(ITableFilter<TData> filter, int offset, int pageSize);

        /// <summary>
        /// Get the filtered table data.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>The filtered table data.</returns>
        Task<IEnumerable<TData>> GetDataAsync(ITableFilter<TData> filter);

        /// <summary>
        /// Get a data page from the sorted table data.
        /// </summary>
        /// <param name="sorting">The sorting to apply.</param>
        /// <param name="offset">The page offset.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The page data.</returns>
        Task<IEnumerable<TData>> GetDataPageAsync(ITableSorting<TData> sorting, int offset, int pageSize);

        /// <summary>
        /// Get the sorted table data.
        /// </summary>
        /// <param name="sorting">The sorting to apply.</param>
        /// <returns>The sorted table data.</returns>
        Task<IEnumerable<TData>> GetDataAsync(ITableSorting<TData> sorting);

        /// <summary>
        /// Get data count.
        /// </summary>
        /// <returns>The data count.</returns>
        Task<int> GetDataCountAsync();

        /// <summary>
        /// Get the filtered data count.
        /// </summary>
        /// <param name="filter">The filter to apply.</param>
        /// <returns></returns>
        Task<int> GetDataCountAsync(ITableFilter<TData> filter);
    }
}
