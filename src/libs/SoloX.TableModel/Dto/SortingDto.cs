// ----------------------------------------------------------------------
// <copyright file="SortingDto.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
namespace SoloX.TableModel.Dto
{
    /// <summary>
    /// Sorting DTO.
    /// </summary>
    public class SortingDto
    {
        /// <summary>
        /// Get/Set the column to sort.
        /// </summary>
        public ColumnDto Column { get; set; }

        /// <summary>
        /// Get/Set the sorting order.
        /// </summary>
        public SortingOrder Order { get; set; }
    }
}
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
