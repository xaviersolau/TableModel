// ----------------------------------------------------------------------
// <copyright file="TableStructureDto.cs" company="Xavier Solau">
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
    /// Table structure DTO.
    /// </summary>
    public class TableStructureDto
    {
        /// <summary>
        /// Get/Set the table Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Get/Set the table items data type.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Get/Set the column Id type.
        /// </summary>
        public string IdType { get; set; }

        /// <summary>
        /// Get/Set the column providing the data item Id.
        /// </summary>
        public ColumnDto IdColumn { get; set; }

        /// <summary>
        /// Get/Set the table data columns.
        /// </summary>
        public IEnumerable<ColumnDto> DataColumns { get; set; }
    }
}
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
