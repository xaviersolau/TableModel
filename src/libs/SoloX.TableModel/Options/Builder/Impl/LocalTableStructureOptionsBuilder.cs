// ----------------------------------------------------------------------
// <copyright file="LocalTableStructureOptionsBuilder.cs" company="Xavier Solau">
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
    /// Local table structure options builder implementation.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TId">Table Id type.</typeparam>
    public class LocalTableStructureOptionsBuilder<TData, TId> : ATableStructureOptionsBuilder, ILocalTableStructureOptionsBuilder<TData, TId>
    {
        private readonly Action<ILocalTableStructureOptions<TData, TId>> tableStructureOptionsSetup;

        private readonly List<ATableDecoratorOptions> tableDecoratorOptions = new List<ATableDecoratorOptions>();

        /// <summary>
        /// Get the Table Structure Id.
        /// </summary>
        public string TableStructureId { get; }

        /// <summary>
        /// Get the associated Decorator options.
        /// </summary>
        public IEnumerable<ATableDecoratorOptions> TableDecoratorOptions => this.tableDecoratorOptions;

        /// <summary>
        /// Setup Local table structure options builder with Id and setup delegate.
        /// </summary>
        /// <param name="tableStructureId">The table structure Id.</param>
        /// <param name="tableStructureOptionsSetup">The setup delegate.</param>
        public LocalTableStructureOptionsBuilder(string tableStructureId, Action<ILocalTableStructureOptions<TData, TId>> tableStructureOptionsSetup)
        {
            TableStructureId = tableStructureId;
            this.tableStructureOptionsSetup = tableStructureOptionsSetup;
        }

        /// <inheritdoc/>
        public override ATableStructureOptions Build()
        {
            var opt = new LocalTableStructureOptions<TData, TId>(TableStructureId, this.tableDecoratorOptions);

            this.tableStructureOptionsSetup(opt);

            return opt;
        }

        /// <inheritdoc/>
        public ILocalTableStructureOptionsBuilder<TData, TId> WithDecorator<TDecorator>(string decoratorId, Action<ILocalTableDecoratorOptions<TData, TDecorator>> tableDecoratorOptionsSetup)
        {
            ArgumentNullException.ThrowIfNull(tableDecoratorOptionsSetup, nameof(tableDecoratorOptionsSetup));

            var decoratorOptions = new LocalTableDecoratorOptions<TData, TId, TDecorator>(TableStructureId, decoratorId);

            tableDecoratorOptionsSetup(decoratorOptions);

            this.tableDecoratorOptions.Add(decoratorOptions);

            return this;
        }
    }
}
