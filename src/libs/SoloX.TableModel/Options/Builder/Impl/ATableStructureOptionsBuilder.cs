// ----------------------------------------------------------------------
// <copyright file="ATableStructureOptionsBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.TableModel.Options.Impl;

namespace SoloX.TableModel.Options.Builder.Impl
{
    /// <summary>
    /// Base abstract class for TableStructure Options Builders
    /// </summary>
    public abstract class ATableStructureOptionsBuilder
    {
        /// <summary>
        /// Build the Table structure options.
        /// </summary>
        /// <returns>The Table structure options.</returns>
        public abstract ATableStructureOptions Build();
    }
}
