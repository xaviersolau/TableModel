// ----------------------------------------------------------------------
// <copyright file="IRemoteTableStructureOptionsBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.TableModel.Options.Builder
{
    /// <summary>
    /// Remote table structure options builder interface.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TId">Table Id type.</typeparam>
    public interface IRemoteTableStructureOptionsBuilder<TData, TId>
    {
        /// <summary>
        /// Add a decorator declaration associated to the current table structure.
        /// </summary>
        /// <typeparam name="TDecorator">The decorator value type.</typeparam>
        /// <param name="decoratorId">The decorator Id.</param>
        /// <param name="tableDecoratorOptionsSetup">Decorator setup delegate.</param>
        /// <returns>The current Local table structure option builder.</returns>
        IRemoteTableStructureOptionsBuilder<TData, TId> WithDecorator<TDecorator>(string decoratorId, Action<ILocalTableDecoratorOptions<TData, TDecorator>> tableDecoratorOptionsSetup);

        /// <summary>
        /// Add a remote decorator declaration associated to the current table structure.
        /// </summary>
        /// <typeparam name="TDecorator">The decorator value type.</typeparam>
        /// <param name="decoratorId">The remote decorator Id.</param>
        /// <param name="tableDecoratorOptionsSetup">Decorator setup delegate.</param>
        /// <returns>The current Local table structure option builder.</returns>
        IRemoteTableStructureOptionsBuilder<TData, TId> WithRemoteDecorator<TDecorator>(string decoratorId, Action<IRemoteTableDecoratorOptions<TData, TDecorator>> tableDecoratorOptionsSetup);
    }
}
