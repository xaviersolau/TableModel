// ----------------------------------------------------------------------
// <copyright file="ITableModelToDtoService.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.TableModel.Dto;

namespace SoloX.TableModel.Services
{
    /// <summary>
    /// Table model to Dto service interface.
    /// </summary>
    public interface ITableModelToDtoService
    {
        /// <summary>
        /// Map the given column to Dto.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <param name="column">The column model to map.</param>
        /// <returns>The created Dto</returns>
        ColumnDto Map<TData>(IColumn<TData> column);

        /// <summary>
        /// Map the given column to Dto.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TColumn">The column value type.</typeparam>
        /// <param name="column">The column model to map.</param>
        /// <returns>The created Dto</returns>
        ColumnDto Map<TData, TColumn>(IColumn<TData, TColumn> column);

        /// <summary>
        /// Map the given table structure to Dto.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <param name="tableStructure">The table structure to map.</param>
        /// <returns>The created Dto</returns>
        TableStructureDto Map<TData, TId>(ITableStructure<TData, TId> tableStructure);

        /// <summary>
        /// Map the given table decorator to Dto.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TDecorator">Decorator value type.</typeparam>
        /// <typeparam name="TColumn">The column value type.</typeparam>
        /// <param name="columnDecorator">The decorator column to map.</param>
        /// <returns>The created Dto</returns>
        ColumnDecoratorDto Map<TData, TDecorator, TColumn>(IColumnDecorator<TData, TDecorator, TColumn> columnDecorator);

        /// <summary>
        /// Map the given decorator column to Dto.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TDecorator">Decorator value type.</typeparam>
        /// <param name="columnDecorator">The decorator column to map.</param>
        /// <returns>The created Dto</returns>
        ColumnDecoratorDto Map<TData, TDecorator>(IColumnDecorator<TData, TDecorator> columnDecorator);

        /// <summary>
        /// Map the given decorator column to Dto.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TDecorator">Decorator value type.</typeparam>
        /// <param name="tableDecorator">The table decorator to map.</param>
        /// <returns>The created Dto</returns>
        TableDecoratorDto Map<TData, TDecorator>(ITableDecorator<TData, TDecorator> tableDecorator);
    }
}
