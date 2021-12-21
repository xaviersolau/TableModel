// ----------------------------------------------------------------------
// <copyright file="ATableStructureOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloX.TableModel.Options.Impl
{
    /// <summary>
    /// Table structure options.
    /// </summary>
    public abstract class ATableStructureOptions
    {
        /// <summary>
        /// Setup table decorator options with Ids.
        /// </summary>
        /// <param name="tableStructureId"></param>
        /// <param name="tableDecoratorOptions">The associated table decorator options.</param>
        protected ATableStructureOptions(string tableStructureId, IEnumerable<ATableDecoratorOptions> tableDecoratorOptions)
        {
            TableStructureId = tableStructureId;
            TableDecoratorOptions = tableDecoratorOptions;
        }

        /// <summary>
        /// Get the table structure Id.
        /// </summary>
        public string TableStructureId { get; }

        /// <summary>
        /// Get the associated decorators options.
        /// </summary>
        public IEnumerable<ATableDecoratorOptions> TableDecoratorOptions { get; }

        /// <summary>
        /// Create table Structure instance from the current options.
        /// </summary>
        /// <param name="serviceProvider">The service provider to build eventual dependencies.</param>
        /// <returns>The created Table Structure instance.</returns>
        public abstract Task<ITableStructure> CreateModelInstanceAsync(IServiceProvider serviceProvider);
    }
}
