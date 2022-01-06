// ----------------------------------------------------------------------
// <copyright file="ITableStructureRepository.cs" company="Xavier Solau">
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
    /// Table structure repository interface.
    /// </summary>
    public interface ITableStructureRepository
    {
        /// <summary>
        /// Get all registered table structure ids.
        /// </summary>
        /// <returns>The registered table structure Ids.</returns>
        Task<IEnumerable<string>> GetTableStructureIdsAsync();

        /// <summary>
        /// Get the registered table structure matching the given tableId (table structure fully typed).
        /// </summary>
        /// <typeparam name="TData">Data type the table structure is based on.</typeparam>
        /// <typeparam name="TId">Id type used to identify data items.</typeparam>
        /// <param name="tableId">The table Id to match.</param>
        /// <returns>The registered table structure.</returns>
        Task<ITableStructure<TData, TId>> GetTableStructureAsync<TData, TId>(string tableId);

        /// <summary>
        /// Get the registered table structure matching the given tableId (table structure fully typed).
        /// </summary>
        /// <typeparam name="TData">Data type the table structure is based on.</typeparam>
        /// <typeparam name="TId">Id type used to identify data items.</typeparam>
        /// <returns>The registered table structure.</returns>
        /// <remarks>TData type name is used as table structure Id.</remarks>
        Task<ITableStructure<TData, TId>> GetTableStructureAsync<TData, TId>();

        /// <summary>
        /// Get the registered table structure matching the given tableId (table structure with unspecified Id type).
        /// </summary>
        /// <typeparam name="TData">Data type the table structure is based on.</typeparam>
        /// <param name="tableId">The table Id to match.</param>
        /// <returns>The registered table structure.</returns>
        Task<ITableStructure<TData>> GetTableStructureAsync<TData>(string tableId);

        /// <summary>
        /// Get the registered table structure matching the given tableId (table structure with unspecified Id type).
        /// </summary>
        /// <typeparam name="TData">Data type the table structure is based on.</typeparam>
        /// <returns>The registered table structure.</returns>
        /// <remarks>TData type name is used as table structure Id.</remarks>
        Task<ITableStructure<TData>> GetTableStructureAsync<TData>();

        /// <summary>
        /// Get the registered table structure matching the given tableId (returning table structure base interface).
        /// </summary>
        /// <param name="tableId">The table Id to match.</param>
        /// <returns>The registered table structure.</returns>
        Task<ITableStructure> GetTableStructureAsync(string tableId);

        /// <summary>
        /// Get the registered table decorator matching the given table structure Id and the given table decorator Id.
        /// </summary>
        /// <typeparam name="TData">Data type the table structure is based on.</typeparam>
        /// <typeparam name="TDecorator">Decorator data type.</typeparam>
        /// <param name="tableId">The table structure Id.</param>
        /// <param name="decoratorId">The table decorator Id.</param>
        /// <returns>The matching table decorator.</returns>
        Task<ITableDecorator<TData, TDecorator>> GetTableDecoratorAsync<TData, TDecorator>(string tableId, string decoratorId);

        /// <summary>
        /// Get the registered table decorator matching the given table structure Id and the given table decorator Id.
        /// </summary>
        /// <typeparam name="TData">Data type the table structure is based on.</typeparam>
        /// <typeparam name="TDecorator">Decorator data type.</typeparam>
        /// <param name="decoratorId">The table decorator Id.</param>
        /// <returns>The matching table decorator.</returns>
        /// <remarks>TData type name is used as table structure Id.</remarks>
        Task<ITableDecorator<TData, TDecorator>> GetTableDecoratorAsync<TData, TDecorator>(string decoratorId);

        /// <summary>
        /// Get the registered table decorator matching the given table structure Id and the given table decorator Id (retuning table decorator base interface).
        /// </summary>
        /// <param name="tableId">The table structure Id.</param>
        /// <param name="decoratorId">The table decorator Id.</param>
        /// <returns>The matching table decorator.</returns>
        Task<ITableDecorator> GetTableDecoratorAsync(string tableId, string decoratorId);

        /// <summary>
        /// Register a table structure.
        /// </summary>
        /// <typeparam name="TData">Data type the table structure is based on.</typeparam>
        /// <typeparam name="TId">Id type used to identify the table data items.</typeparam>
        /// <param name="table">The table structure to register.</param>
        void Register<TData, TId>(ITableStructure<TData, TId> table);

        /// <summary>
        /// Register a table decorator.
        /// </summary>
        /// <typeparam name="TData">Data type the table structure is based on.</typeparam>
        /// <typeparam name="TDecorator">Decorator data type.</typeparam>
        /// <param name="decorator">The table decorator to register.</param>
        void Register<TData, TDecorator>(ITableDecorator<TData, TDecorator> decorator);

    }
}
