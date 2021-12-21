// ----------------------------------------------------------------------
// <copyright file="ILocalTableStructureOptionsBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.TableModel.Options.Builder
{
    /// <summary>
    /// Local table structure option builder interface.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TId">Table Id type.</typeparam>
    public interface ILocalTableStructureOptionsBuilder<TData, TId>
    {
        /// <summary>
        /// Add a decorator declaration associated to the current table structure.
        /// </summary>
        /// <typeparam name="TDecorator">The decorator value type.</typeparam>
        /// <param name="decoratorId">The decorator Id.</param>
        /// <param name="tableDecoratorOptionsSetup">Decorator setup delegate.</param>
        /// <returns>The current Local table structure option builder.</returns>
        ILocalTableStructureOptionsBuilder<TData, TId> WithDecorator<TDecorator>(string decoratorId, Action<ILocalTableDecoratorOptions<TData, TDecorator>> tableDecoratorOptionsSetup);
    }
}
