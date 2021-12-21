// ----------------------------------------------------------------------
// <copyright file="IQueryableTableDataOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.TableModel.Options
{
    /// <summary>
    /// Queriable table data options interface.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TQueryableTableData">Queriable table data type.</typeparam>
    public interface IQueryableTableDataOptions<TData, TQueryableTableData>
    where TQueryableTableData : ITableData<TData>
    {
        /// <summary>
        /// Get/Set the factory delegate to create the Queryable Table Data instance.
        /// </summary>
        Func<string, IServiceProvider, TQueryableTableData> Factory { get; set; }
    }
}