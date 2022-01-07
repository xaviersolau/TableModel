// ----------------------------------------------------------------------
// <copyright file="TableStructureEndPointService.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.TableModel.Dto;
using SoloX.TableModel.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloX.TableModel.Server.Services.Impl
{
    /// <summary>
    /// TableStructure End-Point service implementation.
    /// </summary>
    public class TableStructureEndPointService : ITableStructureEndPointService
    {
        private readonly ITableStructureRepository tableStructureRepository;
        private readonly ITableModelToDtoService tableModelToDtoService;

        /// <summary>
        /// Setup TableStructureEndPointService instance.
        /// </summary>
        /// <param name="tableStructureRepository">The table structure repository.</param>
        /// <param name="tableModelToDtoService">The table model to dto service.</param>
        public TableStructureEndPointService(ITableStructureRepository tableStructureRepository, ITableModelToDtoService tableModelToDtoService)
        {
            this.tableStructureRepository = tableStructureRepository;
            this.tableModelToDtoService = tableModelToDtoService;
        }


        /// <inheritdoc/>
        public Task<IEnumerable<string>> GetRegisteredTableStructuresAsync()
        {
            return this.tableStructureRepository.GetTableStructureIdsAsync();
        }

        /// <inheritdoc/>
        public async Task<TableStructureDto> GetTableStructureAsync(string id)
        {
            var tableStructure = await this.tableStructureRepository.GetTableStructureAsync(id).ConfigureAwait(false);

            return tableStructure.Accept(new StructureVisitor(this.tableModelToDtoService));
        }

        private class StructureVisitor : ITableStructureVisitor<TableStructureDto>
        {
            private readonly ITableModelToDtoService tableModelToDtoService;

            public StructureVisitor(ITableModelToDtoService tableModelToDtoService)
            {
                this.tableModelToDtoService = tableModelToDtoService;
            }

            public TableStructureDto Visit<TData, TId>(ITableStructure<TData, TId> tableStructure)
            {
                return this.tableModelToDtoService.Map(tableStructure);
            }
        }
    }
}
