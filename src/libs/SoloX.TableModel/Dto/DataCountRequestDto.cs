// ----------------------------------------------------------------------
// <copyright file="DataCountRequestDto.cs" company="Xavier Solau">
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
    /// Data count request DTO.
    /// </summary>
    public class DataCountRequestDto
    {
        /// <summary>
        /// Get/Set filters to apply on counting.
        /// </summary>
        public IEnumerable<FilterDto>? Filters { get; set; }
    }
}
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
