// ----------------------------------------------------------------------
// <copyright file="RemoteTableDecoratorOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SoloX.TableModel.Services;
using SoloX.TableModel.Dto;
using System.Net.Http.Json;

namespace SoloX.TableModel.Options.Impl
{
    /// <summary>
    /// Remote table data options.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TId">Table Id type.</typeparam>
    /// <typeparam name="TDecorator">Decorator value type.</typeparam>
    public class RemoteTableDecoratorOptions<TData, TId, TDecorator> : ATableDecoratorOptions, IRemoteTableDecoratorOptions<TData, TDecorator>
    {
        /// <inheritdoc/>
        public HttpClient HttpClient { get; set; }

        /// <summary>
        /// Setup remote table decorator options.
        /// </summary>
        /// <param name="tableStructureId">Table structure Id.</param>
        /// <param name="tableDecoratorId">Table decorator Id.</param>
        public RemoteTableDecoratorOptions(string tableStructureId, string tableDecoratorId)
            : base(tableStructureId, tableDecoratorId)
        {
        }

        /// <inheritdoc/>
        public override async Task<ITableDecorator> CreateModelInstanceAsync(IServiceProvider serviceProvider, ITableStructureRepository tableStructureRepository)
        {
            if (tableStructureRepository == null)
            {
                throw new ArgumentNullException(nameof(tableStructureRepository));
            }

            var tableStructure = await tableStructureRepository.GetTableStructureAsync<TData>(TableStructureId).ConfigureAwait(false);

            var dtoToTableModelService = serviceProvider.GetRequiredService<IDtoToTableModelService>();

            var tableDecoratorDto = await HttpClient.GetFromJsonAsync<TableDecoratorDto>($"{TableStructureId}/{TableDecoratorId}").ConfigureAwait(false);

            return dtoToTableModelService.Map<TData, TDecorator>(tableDecoratorDto, tableStructure);
        }
    }
}
