// ----------------------------------------------------------------------
// <copyright file="RemoteTableStructureOptionsBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using SoloX.TableModel.Options.Impl;

#if NETSTANDARD2_1
using ArgumentNullException = SoloX.TableModel.Utils.ArgumentNullException;
#endif

namespace SoloX.TableModel.Options.Builder.Impl
{
    /// <summary>
    /// Remote table structure options builder.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TId">Table Id type.</typeparam>
    public class RemoteTableStructureOptionsBuilder<TData, TId> : ATableStructureOptionsBuilder, IRemoteTableStructureOptionsBuilder<TData, TId>
    {
        private readonly Action<IRemoteTableStructureOptions<TData, TId>> tableStructureOptionsSetup;

        private readonly List<ATableDecoratorOptions> tableDecoratorOptions = new List<ATableDecoratorOptions>();

        /// <summary>
        /// Get the table structure Id.
        /// </summary>
        public string TableStructureId { get; }

        /// <summary>
        /// Get the associated Decorator options.
        /// </summary>
        public IEnumerable<ATableDecoratorOptions> TableDecoratorOptions => this.tableDecoratorOptions;

        /// <summary>
        /// Setup Remote table structure.
        /// </summary>
        /// <param name="tableStructureId">Table structure Id.</param>
        /// <param name="tableStructureOptionsSetup">Setup delegate.</param>
        public RemoteTableStructureOptionsBuilder(string tableStructureId, Action<IRemoteTableStructureOptions<TData, TId>> tableStructureOptionsSetup)
        {
            TableStructureId = tableStructureId;
            this.tableStructureOptionsSetup = tableStructureOptionsSetup;
        }

        /// <inheritdoc/>
        public override ATableStructureOptions Build()
        {
            var opt = new RemoteTableStructureOptions<TData, TId>(TableStructureId, this.tableDecoratorOptions);

            this.tableStructureOptionsSetup(opt);

            return opt;
        }

        /// <inheritdoc/>
        public IRemoteTableStructureOptionsBuilder<TData, TId> WithRemoteDecorator<TDecorator>(string decoratorId, Action<IRemoteTableDecoratorOptions<TData, TDecorator>> tableDecoratorOptionsSetup)
        {
            ArgumentNullException.ThrowIfNull(tableDecoratorOptionsSetup, nameof(tableDecoratorOptionsSetup));

            var decoratorOptions = new RemoteTableDecoratorOptions<TData, TId, TDecorator>(TableStructureId, decoratorId);

            tableDecoratorOptionsSetup(decoratorOptions);

            this.tableDecoratorOptions.Add(decoratorOptions);

            return this;
        }

        /// <inheritdoc/>
        public IRemoteTableStructureOptionsBuilder<TData, TId> WithDecorator<TDecorator>(string decoratorId, Action<ILocalTableDecoratorOptions<TData, TDecorator>> tableDecoratorOptionsSetup)
        {
            ArgumentNullException.ThrowIfNull(tableDecoratorOptionsSetup, nameof(tableDecoratorOptionsSetup));

            var decoratorOptions = new LocalTableDecoratorOptions<TData, TId, TDecorator>(TableStructureId, decoratorId);

            tableDecoratorOptionsSetup(decoratorOptions);

            this.tableDecoratorOptions.Add(decoratorOptions);

            return this;
        }
    }
}
