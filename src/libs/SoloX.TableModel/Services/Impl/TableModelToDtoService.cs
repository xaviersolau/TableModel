// ----------------------------------------------------------------------
// <copyright file="TableModelToDtoService.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.ExpressionTools.Transform;
using SoloX.TableModel.Dto;
using System.Linq;

#if NETSTANDARD2_1
using ArgumentNullException = SoloX.TableModel.Utils.ArgumentNullException;
#else
using System;
#endif

namespace SoloX.TableModel.Services.Impl
{
    /// <summary>
    /// Table model to Dto service.
    /// </summary>
    public class TableModelToDtoService : ITableModelToDtoService
    {
        /// <inheritdoc/>
        public ColumnDto Map<TData>(IColumn<TData> column)
        {
            ArgumentNullException.ThrowIfNull(column, nameof(column));

            var visitor = new Visitor<TData>();

            return column.Accept(visitor);
        }

        /// <inheritdoc/>
        public ColumnDto Map<TData, TColumn>(IColumn<TData, TColumn> column)
        {
            ArgumentNullException.ThrowIfNull(column, nameof(column));

            return new ColumnDto()
            {
                Id = column.Id,
                Header = column.Header,
                CanSort = column.CanSort,
                CanFilter = column.CanFilter,
                DataType = column.DataType.AssemblyQualifiedName,
                DataGetterExpression = column.DataGetterExpression.Serialize(),
            };
        }

        /// <inheritdoc/>
        public TableStructureDto Map<TData, TId>(ITableStructure<TData, TId> tableStructure)
        {
            ArgumentNullException.ThrowIfNull(tableStructure, nameof(tableStructure));

            var visitor = new Visitor<TData>();

            return new TableStructureDto()
            {
                Id = tableStructure.Id,
                DataType = typeof(TData).AssemblyQualifiedName,
                IdType = typeof(TId).AssemblyQualifiedName,

                IdColumn = this.Map(tableStructure.IdColumn),

                DataColumns = tableStructure.DataColumns.Select(c => c.Accept(visitor)),
            };
        }

        /// <inheritdoc/>
        public ColumnDecoratorDto Map<TData, TDecorator>(IColumnDecorator<TData, TDecorator> columnDecorator)
        {
            ArgumentNullException.ThrowIfNull(columnDecorator, nameof(columnDecorator));

            var visitor = new DecoratorVisitor<TData, TDecorator>();

            return columnDecorator.Accept(visitor);
        }

        /// <inheritdoc/>
        public ColumnDecoratorDto Map<TData, TDecorator, TColumn>(IColumnDecorator<TData, TDecorator, TColumn> columnDecorator)
        {
            ArgumentNullException.ThrowIfNull(columnDecorator, nameof(columnDecorator));

            return new ColumnDecoratorDto()
            {
                HeaderDecoratorExpression = columnDecorator.HeaderDecoratorExpression.Serialize(),
                DecoratorExpression = columnDecorator.RelativeDecoratorExpression.Serialize(),
                Id = columnDecorator.Column.Id,
            };
        }

        /// <inheritdoc/>
        public TableDecoratorDto Map<TData, TDecorator>(ITableDecorator<TData, TDecorator> tableDecorator)
        {
            ArgumentNullException.ThrowIfNull(tableDecorator, nameof(tableDecorator));

            var visitor = new DecoratorVisitor<TData, TDecorator>();

            return new TableDecoratorDto()
            {
                Id = tableDecorator.Id,
                DecoratorType = tableDecorator.DecoratorType.AssemblyQualifiedName,
                DecoratorColumns = tableDecorator.TableColumnDecorators.Select(x => x.Accept(visitor)),
                DefaultDecoratorExpression = tableDecorator.DefaultDecoratorExpression.Serialize(),
                DefaultHeaderDecoratorExpression = tableDecorator.DefaultHeaderDecoratorExpression.Serialize(),
            };
        }

        private sealed class DecoratorVisitor<TData, TDecorator> : IColumnDecoratorVisitor<TData, TDecorator, ColumnDecoratorDto>
        {
            public ColumnDecoratorDto Visit<TColumn>(IColumnDecorator<TData, TDecorator, TColumn> columnDecorator)
            {
                return new ColumnDecoratorDto()
                {
                    DecoratorExpression = columnDecorator.RelativeDecoratorExpression.Serialize(),
                    Id = columnDecorator.Column.Id,
                };
            }
        }

        private sealed class Visitor<TData> : IColumnVisitor<TData, ColumnDto>
        {
            public ColumnDto Visit<TColumn>(IColumn<TData, TColumn> column)
            {
                return new ColumnDto()
                {
                    Id = column.Id,
                    Header = column.Header,
                    CanSort = column.CanSort,
                    CanFilter = column.CanFilter,
                    DataType = column.DataType.AssemblyQualifiedName,
                    DataGetterExpression = column.DataGetterExpression.Serialize(),
                };
            }
        }
    }
}
