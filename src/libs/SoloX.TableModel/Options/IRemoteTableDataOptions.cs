// ----------------------------------------------------------------------
// <copyright file="IRemoteTableDataOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Net.Http;

namespace SoloX.TableModel.Options
{
    /// <summary>
    /// Remote table data options interface.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public interface IRemoteTableDataOptions<TData>
    {
        /// <summary>
        /// Get/Set HttpClient
        /// </summary>
        HttpClient HttpClient { get; set; }

        /// <summary>
        /// Get/Set Http Data Suffix.
        /// </summary>
        string HttpDataSuffix { get; set; }

        /// <summary>
        /// Get/Set Http Count Suffix.
        /// </summary>
        string HttpCountSuffix { get; set; }
    }
}
