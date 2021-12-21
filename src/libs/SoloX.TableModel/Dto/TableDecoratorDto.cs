// ----------------------------------------------------------------------
// <copyright file="TableDecoratorDto.cs" company="Xavier Solau">
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
    /// Table decorator DTO.
    /// </summary>
    public class TableDecoratorDto
    {
        /// <summary>
        /// Get/Set the decorator Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Get/Set the target decorator data type.
        /// </summary>
        public string DecoratorType { get; set; }

        /// <summary>
        /// Get/Set the default expression to convert the column data to the target decorator data type.
        /// </summary>
        public string DefaultDecoratorExpression { get; set; }

        /// <summary>
        /// Get/Set the expressions to convert the specified columns data to the target decorator data type.
        /// </summary>
        public IEnumerable<ColumnDecoratorDto> DecoratorColumns { get; set; }
    }
}
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
