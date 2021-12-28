// ----------------------------------------------------------------------
// <copyright file="ATableDataOptions.cs" company="Xavier Solau">
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
    /// Table data options base class.
    /// </summary>
    public abstract class ATableDataOptions
    {
        /// <summary>
        /// Setup instance with Id.
        /// </summary>
        /// <param name="tableDataId">Table data Id.</param>
        /// <param name="dataType">Data type.</param>
        protected ATableDataOptions(string tableDataId, Type dataType)
        {
            TableDataId = tableDataId;
            DataType = dataType;
        }

        /// <summary>
        /// Get Table data Id
        /// </summary>
        public string TableDataId { get; }

        /// <summary>
        /// Get Table data type.
        /// </summary>
        public Type DataType { get; }

        /// <summary>
        /// Create table Data instance from the current options.
        /// </summary>
        /// <param name="serviceProvider">The service provider to build eventual dependencies.</param>
        /// <returns>The created Table Data instance.</returns>
        public abstract Task<ITableData> CreateModelInstanceAsync(IServiceProvider serviceProvider);
    }
}
