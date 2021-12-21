// ----------------------------------------------------------------------
// <copyright file="DataRequestDto.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;

#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
namespace SoloX.TableModel.Dto
{
    /// <summary>
    /// Data request DTO.
    /// </summary>
    public class DataRequestDto
    {
        /// <summary>
        /// Get/Set page offset where to start loading data from.
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Get/Set page size to load.
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// Get/Set sorting options if any.
        /// </summary>
        public IEnumerable<SortingDto> Sortings { get; set; }

        /// <summary>
        /// Get/Set filters to apply on the data.
        /// </summary>
        public IEnumerable<FilterDto> Filters { get; set; }
    }
}
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
