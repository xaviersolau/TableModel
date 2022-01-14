// ----------------------------------------------------------------------
// <copyright file="TableDataControllerBase.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using SoloX.TableModel.Dto;
using System.Threading.Tasks;
using SoloX.TableModel.Server.Services;
using System.Linq;
using System.Collections.Generic;

namespace SoloX.TableModel.Server
{
    /// <summary>
    /// Table data controller base implementation.
    /// </summary>
    public abstract class TableDataControllerBase : ControllerBase
    {
        private readonly ITableDataEndPointService tableDataEndPointService;

        /// <summary>
        /// Setup the table model controller.
        /// </summary>
        /// <param name="tableDataEndPointService">The table data end point service.</param>
        protected TableDataControllerBase(ITableDataEndPointService tableDataEndPointService)
        {
            this.tableDataEndPointService = tableDataEndPointService;
        }

        /// <summary>
        /// Enable AsyncEnumerable Data Streaming.
        /// </summary>
        protected bool EnableAsyncEnumerableDataStreaming { get; set; }

        /// <summary>
        /// Get all registered table data declarations.
        /// </summary>
        /// <returns>The registered table data declarations.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TableDataDto>), 200)]
        public async Task<IActionResult> IndexAsync()
        {
            var tables = await this.tableDataEndPointService.GetRegisteredTableDataAsync().ConfigureAwait(false);
            return Ok(tables);
        }

        /// <summary>
        /// Post a data request on the table data matching the given table data Id.
        /// </summary>
        /// <param name="id">The table data Id to request.</param>
        /// <param name="request">The data request to run.</param>
        /// <returns>The requested data.</returns>
        [HttpPost("{id}/data")]
        [ProducesResponseType(typeof(IEnumerable<object>), 200)]
        public async Task<IActionResult> PostDataRequestAsync(string id, [FromBody] DataRequestDto request)
        {
            var tableData = await this.tableDataEndPointService.ProcessDataRequestAsync<object>(id, request).ConfigureAwait(false);

            if (!EnableAsyncEnumerableDataStreaming)
            {
                tableData = tableData.ToArray();
            }

            return Ok(tableData);
        }

        /// <summary>
        /// Post a data count request on the table data matching the given table data Id.
        /// </summary>
        /// <param name="id">The table data Id to request.</param>
        /// <param name="request">The data count request to run.</param>
        /// <returns>The requested data count.</returns>
        [HttpPost("{id}/count")]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> PostDataCountRequestAsync(string id, [FromBody] DataCountRequestDto request)
        {
            var tableDataCount = await this.tableDataEndPointService.ProcessDataCountRequestAsync(id, request).ConfigureAwait(false);

            return Ok(tableDataCount);
        }
    }
}