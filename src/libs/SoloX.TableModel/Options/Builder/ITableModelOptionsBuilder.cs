// ----------------------------------------------------------------------
// <copyright file="ITableModelOptionsBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.TableModel.Options.Builder
{
    /// <summary>
    /// Table model options builder interface.
    /// </summary>
    public interface ITableModelOptionsBuilder
    {
        /// <summary>
        /// Declare the use of a local table structure.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TId">Table Id type.</typeparam>
        /// <param name="tableId">Table Id.</param>
        /// <param name="configAction">Setup delegate.</param>
        /// <returns>The current table model builder.</returns>
        ILocalTableStructureOptionsBuilder<TData, TId> UseTableStructure<TData, TId>(string tableId, Action<ILocalTableStructureOptions<TData, TId>> configAction);

        /// <summary>
        /// Declare the use of a local table structure.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TId">Table Id type.</typeparam>
        /// <param name="configAction">Setup delegate.</param>
        /// <returns>The current table model builder.</returns>
        /// <remarks>TData type name is used as Table structure ID.</remarks>
        ILocalTableStructureOptionsBuilder<TData, TId> UseTableStructure<TData, TId>(Action<ILocalTableStructureOptions<TData, TId>> configAction);

        /// <summary>
        /// Declare the use of a remote table structure.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TId">Table Id type.</typeparam>
        /// <param name="configAction">Setup delegate.</param>
        /// <returns>The current table model builder.</returns>
        IRemoteTableStructureOptionsBuilder<TData, TId> UseRemoteTableStructure<TData, TId>(string tableId, Action<IRemoteTableStructureOptions<TData, TId>> configAction);

        /// <summary>
        /// Declare the use of a remote table structure.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TId">Table Id type.</typeparam>
        /// <param name="configAction">Setup delegate.</param>
        /// <returns>The current table model builder.</returns>
        /// <remarks>TData type name is used as Table structure ID.</remarks>
        IRemoteTableStructureOptionsBuilder<TData, TId> UseRemoteTableStructure<TData, TId>(Action<IRemoteTableStructureOptions<TData, TId>> configAction);

        /// <summary>
        /// Declare the use of a queriable table data.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TQueryableTableData">Queriable Table data type.</typeparam>
        /// <param name="tableId">Table Id.</param>
        /// <param name="configAction">Setup delegate.</param>
        /// <returns>The current table model builder.</returns>
        IQueryableTableDataOptionsBuilder<TData, TQueryableTableData> UseQueryableTableData<TData, TQueryableTableData>(
            string tableId,
            Action<IQueryableTableDataOptions<TData, TQueryableTableData>> configAction)
            where TQueryableTableData : ITableData<TData>;

        /// <summary>
        /// Declare the use of a queriable table data.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TQueryableTableData">Queriable Table data type.</typeparam>
        /// <param name="configAction">Setup delegate.</param>
        /// <returns>The current table model builder.</returns>
        /// <remarks>TData type name is used as Table ID.</remarks>
        IQueryableTableDataOptionsBuilder<TData, TQueryableTableData> UseQueryableTableData<TData, TQueryableTableData>(
            Action<IQueryableTableDataOptions<TData, TQueryableTableData>> configAction)
            where TQueryableTableData : ITableData<TData>;

        /// <summary>
        /// Declare the use of a in memory table data.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <param name="tableId">Table Id.</param>
        /// <param name="configAction">Setup delegate.</param>
        /// <returns>The current table model builder.</returns>
        IMemoryTableDataOptionsBuilder<TData> UseMemoryTableData<TData>(string tableId, Action<IMemoryTableDataOptions<TData>> configAction);

        /// <summary>
        /// Declare the use of a in memory table data.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <param name="configAction">Setup delegate.</param>
        /// <returns>The current table model builder.</returns>
        /// <remarks>TData type name is used as Table ID.</remarks>
        IMemoryTableDataOptionsBuilder<TData> UseMemoryTableData<TData>(Action<IMemoryTableDataOptions<TData>> configAction);

        /// <summary>
        /// Declare the use of a remote table data.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <param name="tableId">Table Id.</param>
        /// <param name="configAction">Setup delegate.</param>
        /// <returns>The current table model builder.</returns>
        IRemoteTableDataOptionsBuilder<TData> UseRemoteTableData<TData>(string tableId, Action<IRemoteTableDataOptions<TData>> configAction);

        /// <summary>
        /// Declare the use of a remote table data.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <param name="configAction">Setup delegate.</param>
        /// <returns>The current table model builder.</returns>
        /// <remarks>TData type name is used as Table ID.</remarks>
        IRemoteTableDataOptionsBuilder<TData> UseRemoteTableData<TData>(Action<IRemoteTableDataOptions<TData>> configAction);
    }
}
