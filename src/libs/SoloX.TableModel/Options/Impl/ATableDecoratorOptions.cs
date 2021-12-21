// ----------------------------------------------------------------------
// <copyright file="ATableDecoratorOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace SoloX.TableModel.Options.Impl
{
    /// <summary>
    /// Table decorator options.
    /// </summary>
    public abstract class ATableDecoratorOptions
    {
        /// <summary>
        /// Get the table decorator Id.
        /// </summary>
        public string TableDecoratorId { get; }

        /// <summary>
        /// Get the associated table structure Id.
        /// </summary>
        public string TableStructureId { get; }

        /// <summary>
        /// Setup table decorator options with Ids.
        /// </summary>
        /// <param name="tableStructureId"></param>
        /// <param name="tableDecoratorId"></param>
        protected ATableDecoratorOptions(string tableStructureId, string tableDecoratorId)
        {
            TableStructureId = tableStructureId;
            TableDecoratorId = tableDecoratorId;
        }

        /// <summary>
        /// Create table Decorator instance from the current options.
        /// </summary>
        /// <param name="serviceProvider">The service provider to build eventual dependencies.</param>
        /// <param name="tableStructureRepository">The associated table structure repository.</param>
        /// <returns>The created Table Decorator instance.</returns>
        public abstract Task<ITableDecorator> CreateModelInstanceAsync(IServiceProvider serviceProvider, ITableStructureRepository tableStructureRepository);
    }
}
