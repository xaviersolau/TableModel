// ----------------------------------------------------------------------
// <copyright file="RemoteTableDataOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using SoloX.TableModel.Impl;

namespace SoloX.TableModel.Options.Impl
{
    /// <summary>
    /// Remote table data options.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public class RemoteTableDataOptions<TData> : ATableDataOptions, IRemoteTableDataOptions<TData>
    {
        /// <inheritdoc/>
        public HttpClient HttpClient { get; set; }

        /// <summary>
        /// Setup remote table data options.
        /// </summary>
        /// <param name="tableId">Table data Id.</param>
        public RemoteTableDataOptions(string tableId)
            : base(tableId)
        {
        }

        /// <inheritdoc/>
        public override Task<ITableData> CreateModelInstanceAsync(IServiceProvider serviceProvider)
        {
            return Task.FromResult<ITableData>(new RemoteTableData<TData>(TableDataId, HttpClient));
        }
    }
}
