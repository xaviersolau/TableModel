// ----------------------------------------------------------------------
// <copyright file="ITableDataEndPointService.cs" company="Xavier Solau">
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
    /// TableData End point service interface.
    /// </summary>
    public interface ITableDataEndPointService
    {
        /// <summary>
        /// Process data request.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="id">The table data Id.</param>
        /// <param name="request">The data request Dto.</param>
        /// <returns>The requested data.</returns>
        Task<IEnumerable<TData>> ProcessDataRequestAsync<TData>(string id, DataRequestDto request);

        /// <summary>
        /// Process data count request.
        /// </summary>
        /// <param name="id">The table data Id.</param>
        /// <param name="request">The data count request Dto.</param>
        /// <returns>The requested data count.</returns>
        Task<int> ProcessDataCountRequestAsync(string id, DataCountRequestDto request);

        /// <summary>
        /// Get all registered table data.
        /// </summary>
        /// <returns>All registered table data.</returns>
        Task<IEnumerable<TableDataDto>> GetRegisteredTableDataAsync();
    }
}
