// ----------------------------------------------------------------------
// <copyright file="TableModelExtensions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using SoloX.TableModel.Impl;
using System;
using SoloX.TableModel.Services.Impl;
using SoloX.TableModel.Options.Builder;
using SoloX.TableModel.Options.Builder.Impl;
using SoloX.TableModel.Options.Impl;
using SoloX.TableModel.Services;

namespace SoloX.TableModel
{
    /// <summary>
    /// Table model extension method used to setup dependency injection.
    /// </summary>
    public static class TableModelExtensions
    {
        /// <summary>
        /// Add table model and build the options to use.
        /// </summary>
        /// <param name="services">The service collection to initialize.</param>
        /// <param name="setupAction">Setup option builder delegate.</param>
        public static void AddTableModel(this IServiceCollection services, Action<ITableModelOptionsBuilder> setupAction)
        {
            services.AddScoped<ITableDataRepository, TableDataRepository>();
            services.AddScoped<ITableStructureRepository, TableStructureRepository>();

            services.AddSingleton<ITableFactory, TableFactory>();

            services.AddSingleton<IDtoToTableModelService, DtoToTableModelService>();
            services.AddSingleton<ITableModelToDtoService, TableModelToDtoService>();

            var tableModelOptionsBuilder = new TableModelOptionsBuilder(setupAction);

            services.Configure<TableModelOptions>(tableModelOptionsBuilder.Build);
        }
    }
}
