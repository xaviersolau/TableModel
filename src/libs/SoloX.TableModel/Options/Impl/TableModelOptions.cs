// ----------------------------------------------------------------------
// <copyright file="TableModelOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;

namespace SoloX.TableModel.Options.Impl
{
    /// <summary>
    /// Table model options.
    /// </summary>
    public class TableModelOptions
    {
        /// <summary>
        /// Get Table structure options.
        /// </summary>
        public IEnumerable<ATableStructureOptions> TableStructureOptions { get; internal set; }

        /// <summary>
        /// Get Table data options.
        /// </summary>
        public IEnumerable<ATableDataOptions> TableDataOptions { get; internal set; }
    }
}
