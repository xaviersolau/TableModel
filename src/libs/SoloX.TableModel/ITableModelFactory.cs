// ----------------------------------------------------------------------
// <copyright file="ITableModelFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.TableModel
{
    /// <summary>
    /// Table model factory interface.
    /// </summary>
    public interface ITableModelFactory
    {
        /// <summary>
        /// Create a Table filter.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <returns>The created Table Filter instance.</returns>
        ITableFilter<TData> CreateTableFilter<TData>();

        /// <summary>
        /// Create a Table Sorting.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <returns>The created Table Sorting instance.</returns>
        ITableSorting<TData> CreateTableSorting<TData>();
    }
}
