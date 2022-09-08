// ----------------------------------------------------------------------
// <copyright file="FilterDto.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
namespace SoloX.TableModel.Dto
{
    /// <summary>
    /// Filter DTO.
    /// </summary>
    public class FilterDto
    {
        /// <summary>
        /// Get/Set the column to filter.
        /// </summary>
        public ColumnDto? Column { get; set; }

        /// <summary>
        /// Get/Set the filter expression.
        /// </summary>
        public string FilterExpression { get; set; }
    }
}
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
