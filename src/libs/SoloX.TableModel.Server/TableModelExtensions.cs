// ----------------------------------------------------------------------
// <copyright file="TableModelExtensions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using SoloX.TableModel.Options.Builder;
using SoloX.TableModel.Server.Services;
using SoloX.TableModel.Server.Services.Impl;
using System;

namespace SoloX.TableModel.Server
{
    /// <summary>
    /// Table model server extension method used to setup dependency injection.
    /// </summary>
    public static class TableModelExtensions
    {
        /// <summary>
        /// Add table model server and build the options to use.
        /// </summary>
        /// <param name="services">The service collection to initialize.</param>
        /// <param name="setupAction">Setup option builder delegate.</param>
        public static void AddTableModelServer(this IServiceCollection services, Action<ITableModelOptionsBuilder> setupAction)
        {
            services.AddTransient<ITableDataEndPointService, TableDataEndPointService>();
            services.AddTableModel(setupAction);
        }

    }
}
