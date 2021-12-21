// ----------------------------------------------------------------------
// <copyright file="IColumnSorting.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Linq;

namespace SoloX.TableModel
{
    /// <summary>
    /// Table column sorting interface.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IColumnSorting<TData>
    {
        /// <summary>
        /// Get the associated column declaration.
        /// </summary>
        IColumn<TData> Column { get; }

        /// <summary>
        /// Get the sorting order.
        /// </summary>
        SortingOrder Order { get; }

        /// <summary>
        /// Apply the sort onto the given table queryable.
        /// </summary>
        /// <param name="data">Table item data type.</param>
        /// <returns>The sorted queryable.</returns>
        IQueryable<TData> Apply(IQueryable<TData> data);
    }
}
