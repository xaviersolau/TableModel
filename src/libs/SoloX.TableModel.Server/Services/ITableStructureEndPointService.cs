// ----------------------------------------------------------------------
// <copyright file="ITableStructureEndPointService.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.TableModel.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloX.TableModel.Server.Services
{
    /// <summary>
    /// ITableStructure End point service interface.
    /// </summary>
    public interface ITableStructureEndPointService
    {
        /// <summary>
        /// Get all available table structure Ids.
        /// </summary>
        /// <returns>All available table structure Ids.</returns>
        Task<IEnumerable<string>> GetRegisteredTableStructuresAsync();

        /// <summary>
        /// Get the table structure matching the given Id.
        /// </summary>
        /// <param name="id">Table structure to search.</param>
        /// <returns>The matching table structure.</returns>
        Task<TableStructureDto> GetTableStructureAsync(string id);
    }
}
