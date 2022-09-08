// ----------------------------------------------------------------------
// <copyright file="IDataFilter.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq;
using System.Linq.Expressions;

namespace SoloX.TableModel
{
    /// <summary>
    /// Data filter descriptor.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public interface IDataFilter<TData>
    {
        /// <summary>
        /// Lambda expression defining the filter to apply from the table data item.
        /// </summary>
        Expression<Func<TData, bool>> DataFilter { get; }

        /// <summary>
        /// Apply the filter onto the given table queryable.
        /// </summary>
        /// <param name="data">The queryable where to apply the filter.</param>
        /// <returns>The filtered queryable.</returns>
        IQueryable<TData> Apply(IQueryable<TData> data);
    }
}
