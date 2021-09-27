using SoloX.TableModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Services.Impl
{
    public class TableModelToDtoService : ITableModelToDtoService
    {
        public ColumnDto Map<TData>(IColumn<TData> column)
        {
            var visitor = new Visitor<TData>();

            return column.Accept(visitor);
        }

        public ColumnDto Map<TData, TColumn>(IColumn<TData, TColumn> column)
        {
            return new ColumnDto()
            {
                Id = column.Id,
                CanSort = column.CanSort,
                CanFilter = column.CanFilter,
                DataType = column.DataType.AssemblyQualifiedName,
                DataGetterExpression = column.DataGetterExpression.ToString(),
            };
        }

        public TableStructureDto Map<TData, TId>(ITableStructure<TData, TId> tableStructure)
        {
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

        public ColumnDecoratorDto Map<TData, TDecorator>(IColumnDecorator<TData, TDecorator> columnDecorator)
        {
            var visitor = new DecoratorVisitor<TData, TDecorator>();

            return columnDecorator.Accept(visitor);
        }

        public ColumnDecoratorDto Map<TData, TDecorator, TColumn>(IColumnDecorator<TData, TDecorator, TColumn> columnDecorator)
        {
            return new ColumnDecoratorDto()
            {
                DecoratorExpression = columnDecorator.RelativeDecoratorExpression.ToString(),
                Id = columnDecorator.Column.Id,
            };
        }

        public TableDecoratorDto Map<TData, TDecorator>(ITableDecorator<TData, TDecorator> tableDecorator)
        {
            var visitor = new DecoratorVisitor<TData, TDecorator>();

            return new TableDecoratorDto()
            {
                Id = tableDecorator.Id,
                DecoratorType = tableDecorator.DecoratorType.AssemblyQualifiedName,
                DecoratorColumns = tableDecorator.TableColumnDecorators.Select(x => x.Accept(visitor)),
                DefaultDecoratorExpression = tableDecorator.DefaultDecoratorExpression.ToString(),
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
                    CanSort = column.CanSort,
                    CanFilter = column.CanFilter,
                    DataType = column.DataType.AssemblyQualifiedName,
                    DataGetterExpression = column.DataGetterExpression.ToString(),
                };
            }
        }
    }
}
