// ----------------------------------------------------------------------
// <copyright file="RemoteTableStructureOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.TableModel.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SoloX.TableModel.Dto;
using System.Net.Http.Json;

namespace SoloX.TableModel.Options.Impl
{
    /// <summary>
    /// Remote table structure options.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TId">Table Id type.</typeparam>
    public class RemoteTableStructureOptions<TData, TId> : ATableStructureOptions, IRemoteTableStructureOptions<TData, TId>
    {
        /// <inheritdoc/>
        public HttpClient HttpClient { get; set; }

        /// <summary>
        /// Setup RemoteTableStructureOptions with Id and decorator options.
        /// </summary>
        /// <param name="tableStructureId"></param>
        /// <param name="tableDecoratorOptions"></param>
        public RemoteTableStructureOptions(string tableStructureId, IEnumerable<ATableDecoratorOptions> tableDecoratorOptions)
            : base(tableStructureId, tableDecoratorOptions)
        { }

        /// <inheritdoc/>
        public override async Task<ITableStructure> CreateModelInstanceAsync(IServiceProvider serviceProvider)
        {
            var dtoToTableModelService = serviceProvider.GetRequiredService<IDtoToTableModelService>();

            var tableStructureDto = await HttpClient.GetFromJsonAsync<TableStructureDto>(TableStructureId).ConfigureAwait(false);

            return dtoToTableModelService.Map<TData, TId>(tableStructureDto);
        }
    }
}
