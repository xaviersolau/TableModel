// ----------------------------------------------------------------------
// <copyright file="IRemoteTableStructureOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Net.Http;

namespace SoloX.TableModel.Options
{
    /// <summary>
    /// Remote table structure options interface.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TId">Table Id type.</typeparam>
    public interface IRemoteTableStructureOptions<TData, TId>
    {
        /// <summary>
        /// Get/Set HttpClient
        /// </summary>
        HttpClient HttpClient { get; set; }
    }
}
