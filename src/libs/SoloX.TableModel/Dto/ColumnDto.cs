// ----------------------------------------------------------------------
// <copyright file="ColumnDto.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
namespace SoloX.TableModel.Dto
{
    /// <summary>
    /// Column DTO.
    /// </summary>
    public class ColumnDto
    {
        /// <summary>
        /// Get/Set Column Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Get/Set Column data type.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Get/Set CanSort property.
        /// </summary>
        public bool CanSort { get; set; }

        /// <summary>
        /// Get/Set CanFilter property.
        /// </summary>
        public bool CanFilter { get; set; }

        /// <summary>
        /// Get/Set the column data getter expression.
        /// </summary>
        public string DataGetterExpression { get; set; }
    }
}
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
