// ----------------------------------------------------------------------
// <copyright file="IDtoToTableModelService.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.TableModel.Dto;

namespace SoloX.TableModel.Services
{
    /// <summary>
    /// Dto to Table data model service interface.
    /// </summary>
    public interface IDtoToTableModelService
    {
        /// <summary>
        /// Map the given column dto to a Column description model.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <param name="dto">The column dto.</param>
        /// <returns>The created column model.</returns>
        IColumn<TData> Map<TData>(ColumnDto dto);

        /// <summary>
        /// Map the given table structure dto to a table structure description model.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <param name="dto">The structure dto.</param>
        /// <returns>The created table structure model.</returns>
        ITableStructure<TData> Map<TData>(TableStructureDto dto);

        /// <summary>
        /// Map the given table structure dto to a table structure description model.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TId">Table Id type.</typeparam>
        /// <param name="dto">The structure dto.</param>
        /// <returns>The created table structure model.</returns>
        ITableStructure<TData, TId> Map<TData, TId>(TableStructureDto dto);

        /// <summary>
        /// Map the given table decorator dto to a table decorator description model.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TDecorator">Decorator value type.</typeparam>
        /// <param name="dto">The table decorator dto.</param>
        /// <param name="tableStructure">The associated table structure model.</param>
        /// <returns>The created table decorator model.</returns>
        ITableDecorator<TData, TDecorator> Map<TData, TDecorator>(TableDecoratorDto dto, ITableStructure<TData> tableStructure);

        /// <summary>
        /// Map the given table decorator column dto to a table decorator column description model.
        /// </summary>
        /// <typeparam name="TData">Table data type.</typeparam>
        /// <typeparam name="TDecorator">Decorator value type.</typeparam>
        /// <typeparam name="TColumn">The column value type.</typeparam>
        /// <param name="dto">The decorator column dto.</param>
        /// <param name="column">The associated data column model.</param>
        /// <returns>The created table decorator column model.</returns>
        IColumnDecorator<TData, TDecorator, TColumn> Map<TData, TDecorator, TColumn>(ColumnDecoratorDto dto, IColumn<TData, TColumn> column);
    }
}
