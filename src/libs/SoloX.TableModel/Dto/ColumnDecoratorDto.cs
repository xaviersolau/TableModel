// ----------------------------------------------------------------------
// <copyright file="ColumnDecoratorDto.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

#pragma warning disable CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
namespace SoloX.TableModel.Dto
{
    /// <summary>
    /// Column decorator DTO.
    /// </summary>
    public class ColumnDecoratorDto
    {
        /// <summary>
        /// Get/Set Column Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Get/Set Decorator expression.
        /// </summary>
        public string DecoratorExpression { get; set; }
    }
}
#pragma warning restore CS8618 // Un champ non-nullable doit contenir une valeur non-null lors de la fermeture du constructeur. Envisagez de déclarer le champ comme nullable.
