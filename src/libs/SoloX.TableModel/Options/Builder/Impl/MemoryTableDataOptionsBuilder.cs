// ----------------------------------------------------------------------
// <copyright file="MemoryTableDataOptionsBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using SoloX.TableModel.Options.Impl;

namespace SoloX.TableModel.Options.Builder.Impl
{
    /// <summary>
    /// In Memory table data options builder.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public class MemoryTableDataOptionsBuilder<TData> : ATableDataOptionsBuilder, IMemoryTableDataOptionsBuilder<TData>
    {
        private readonly Action<IMemoryTableDataOptions<TData>> tableDataOptionsSetup;

        /// <summary>
        /// Setup In memory table data options builder with Id and setup delegate.
        /// </summary>
        /// <param name="tableDataId">The table data Id.</param>
        /// <param name="tableDataOptionsSetup">The setup delegate.</param>
        public MemoryTableDataOptionsBuilder(string tableDataId, Action<IMemoryTableDataOptions<TData>> tableDataOptionsSetup)
            : base(tableDataId)
        {
            this.tableDataOptionsSetup = tableDataOptionsSetup;
        }

        /// <inheritdoc/>
        public override ATableDataOptions Build()
        {
            var opt = new MemoryTableDataOptions<TData>(TableDataId);

            this.tableDataOptionsSetup(opt);

            return opt;
        }
    }
}
