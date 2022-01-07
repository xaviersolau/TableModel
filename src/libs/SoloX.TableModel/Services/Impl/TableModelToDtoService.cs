// ----------------------------------------------------------------------
// <copyright file="TableModelToDtoService.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.TableModel.Dto;
using System;
using System.Linq;

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
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            var visitor = new Visitor<TData>();

            return column.Accept(visitor);
        }

        /// <inheritdoc/>
        public ColumnDto Map<TData, TColumn>(IColumn<TData, TColumn> column)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            return new ColumnDto()
            {
                Id = column.Id,
                Header = column.Header,
                CanSort = column.CanSort,
                CanFilter = column.CanFilter,
                DataType = column.DataType.AssemblyQualifiedName,
                DataGetterExpression = column.DataGetterExpression.ToString(),
            };
        }

        /// <inheritdoc/>
        public TableStructureDto Map<TData, TId>(ITableStructure<TData, TId> tableStructure)
        {
            if (tableStructure == null)
            {
                throw new ArgumentNullException(nameof(tableStructure));
            }

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
            if (columnDecorator == null)
            {
                throw new ArgumentNullException(nameof(columnDecorator));
            }

            var visitor = new DecoratorVisitor<TData, TDecorator>();

            return columnDecorator.Accept(visitor);
        }

        /// <inheritdoc/>
        public ColumnDecoratorDto Map<TData, TDecorator, TColumn>(IColumnDecorator<TData, TDecorator, TColumn> columnDecorator)
        {
            if (columnDecorator == null)
            {
                throw new ArgumentNullException(nameof(columnDecorator));
            }

            return new ColumnDecoratorDto()
            {
                HeaderDecoratorExpression = columnDecorator.HeaderDecoratorExpression.ToString(),
                DecoratorExpression = columnDecorator.RelativeDecoratorExpression.ToString(),
                Id = columnDecorator.Column.Id,
            };
        }

        /// <inheritdoc/>
        public TableDecoratorDto Map<TData, TDecorator>(ITableDecorator<TData, TDecorator> tableDecorator)
        {
            if (tableDecorator == null)
            {
                throw new ArgumentNullException(nameof(tableDecorator));
            }

            var visitor = new DecoratorVisitor<TData, TDecorator>();

            return new TableDecoratorDto()
            {
                Id = tableDecorator.Id,
                DecoratorType = tableDecorator.DecoratorType.AssemblyQualifiedName,
                DecoratorColumns = tableDecorator.TableColumnDecorators.Select(x => x.Accept(visitor)),
                DefaultDecoratorExpression = tableDecorator.DefaultDecoratorExpression.ToString(),
                DefaultHeaderDecoratorExpression = tableDecorator.DefaultHeaderDecoratorExpression.ToString(),
            };
        }

        private class DecoratorVisitor<TData, TDecorator> : IColumnDecoratorVisitor<TData, TDecorator, ColumnDecoratorDto>
        {
            public ColumnDecoratorDto Visit<TColumn>(IColumnDecorator<TData, TDecorator, TColumn> columnDecorator)
            {
                return new ColumnDecoratorDto()
                {
                    DecoratorExpression = columnDecorator.RelativeDecoratorExpression.ToString(),
                    Id = columnDecorator.Column.Id,
                };
            }
        }

        private class Visitor<TData> : IColumnVisitor<TData, ColumnDto>
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
                    DataGetterExpression = column.DataGetterExpression.ToString(),
                };
            }
        }
    }
}
