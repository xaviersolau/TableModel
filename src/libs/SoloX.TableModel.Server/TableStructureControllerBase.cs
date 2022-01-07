// ----------------------------------------------------------------------
// <copyright file="TableStructureControllerBase.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SoloX.TableModel.Server.Services;

namespace SoloX.TableModel.Server
{
    /// <summary>
    /// Table structure controller base implementation.
    /// </summary>
    public abstract class TableStructureControllerBase : ControllerBase
    {
        private readonly ITableStructureEndPointService tableStructureEndPointService;

        /// <summary>
        /// Setup the table model controller.
        /// </summary>
        /// <param name="tableStructureEndPointService">The table structure end point service.</param>
        protected TableStructureControllerBase(ITableStructureEndPointService tableStructureEndPointService)
        {
            this.tableStructureEndPointService = tableStructureEndPointService;
        }

        /// <summary>
        /// Get all registered table structure declarations.
        /// </summary>
        /// <returns>The registered table structure declarations.</returns>
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var tables = await this.tableStructureEndPointService.GetRegisteredTableStructuresAsync().ConfigureAwait(false);
            return Ok(tables);
        }

        /// <summary>
        /// Get the table structure matching the given table structure Id.
        /// </summary>
        /// <param name="id">The table structure Id to request.</param>
        /// <returns>The requested table structure.</returns>
        [HttpGet("{id}/structure")]
        public async Task<IActionResult> GetTableStructureAsync(string id)
        {
            var tableStructure = await this.tableStructureEndPointService.GetTableStructureAsync(id).ConfigureAwait(false);

            return Ok(tableStructure);
        }
    }
}