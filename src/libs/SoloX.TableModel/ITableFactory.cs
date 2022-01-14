// ----------------------------------------------------------------------
// <copyright file="ITableFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.TableModel
{
    /// <summary>
    /// Table factory interface to create ITableSorting and ITableFilter object.
    /// </summary>
    public interface ITableFactory
    {
        /// <summary>
        /// Create a TableFilter instance.
        /// </summary>
        /// <typeparam name="TData">Table Data type</typeparam>
        /// <returns>The created TableFilter.</returns>
        ITableFilter<TData> CreateTableFilter<TData>();

        /// <summary>
        /// Create a TableSorting instance.
        /// </summary>
        /// <typeparam name="TData">Table Data type</typeparam>
        /// <returns>The created TableSorting.</returns>
        ITableSorting<TData> CreateTableSorting<TData>();
    }
}
