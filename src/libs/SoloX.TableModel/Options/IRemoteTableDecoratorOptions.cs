// ----------------------------------------------------------------------
// <copyright file="IRemoteTableDecoratorOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Net.Http;

namespace SoloX.TableModel.Options
{
    /// <summary>
    /// Remote table decorator options interface.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TDecorator">Decorator value type.</typeparam>
    public interface IRemoteTableDecoratorOptions<TData, TDecorator>
    {
        /// <summary>
        /// Get/Set HttpClient
        /// </summary>
        HttpClient HttpClient { get; set; }
    }
}
