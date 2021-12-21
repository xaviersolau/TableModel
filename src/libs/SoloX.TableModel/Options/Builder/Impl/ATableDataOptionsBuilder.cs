// ----------------------------------------------------------------------
// <copyright file="ATableDataOptionsBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.TableModel.Options.Impl;

namespace SoloX.TableModel.Options.Builder.Impl
{
    /// <summary>
    /// Base abstract class for TableData Options Builders
    /// </summary>
    public abstract class ATableDataOptionsBuilder
    {
        /// <summary>
        /// Get the Table data Id
        /// </summary>
        public string TableDataId { get; }

        /// <summary>
        /// Setup the base class with the table data Id.
        /// </summary>
        /// <param name="tableDataId">The table data Id.</param>
        protected ATableDataOptionsBuilder(string tableDataId)
        {
            TableDataId = tableDataId;
        }

        /// <summary>
        /// Build the Table data options.
        /// </summary>
        /// <returns>The Table data options.</returns>
        public abstract ATableDataOptions Build();
    }
}
