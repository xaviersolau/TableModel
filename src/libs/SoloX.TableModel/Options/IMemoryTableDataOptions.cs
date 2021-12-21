// ----------------------------------------------------------------------
// <copyright file="IMemoryTableDataOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;

namespace SoloX.TableModel.Options
{
    /// <summary>
    /// In memory table data options.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public interface IMemoryTableDataOptions<TData>
    {
        /// <summary>
        /// Add data.
        /// </summary>
        /// <param name="data">Data to add to the In Memory Table data.</param>
        void AddData(IEnumerable<TData> data);
    }
}
