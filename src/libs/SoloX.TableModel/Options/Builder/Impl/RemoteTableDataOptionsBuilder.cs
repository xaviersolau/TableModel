// ----------------------------------------------------------------------
// <copyright file="RemoteTableDataOptionsBuilder.cs" company="Xavier Solau">
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
    /// Remote table data options builder.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public class RemoteTableDataOptionsBuilder<TData> : ATableDataOptionsBuilder, IRemoteTableDataOptionsBuilder<TData>
    {
        private readonly Action<IRemoteTableDataOptions<TData>> tableDataOptionsSetup;

        /// <summary>
        /// Setup Remote table data options builder with Id and setup delegate.
        /// </summary>
        /// <param name="tableDataId">The table data Id.</param>
        /// <param name="tableDataOptionsSetup">The setup delegate.</param>
        public RemoteTableDataOptionsBuilder(string tableDataId, Action<IRemoteTableDataOptions<TData>> tableDataOptionsSetup)
            : base(tableDataId)
        {
            this.tableDataOptionsSetup = tableDataOptionsSetup;
        }

        /// <inheritdoc/>
        public override ATableDataOptions Build()
        {
            var opt = new RemoteTableDataOptions<TData>(TableDataId);

            this.tableDataOptionsSetup(opt);

            return opt;
        }
    }
}
