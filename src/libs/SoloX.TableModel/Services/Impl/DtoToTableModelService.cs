using SoloX.ExpressionTools.Parser;
using SoloX.ExpressionTools.Parser.Impl;
using SoloX.ExpressionTools.Parser.Impl.Resolver;
using SoloX.TableModel.Dto;
using SoloX.TableModel.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Services.Impl
{
    public class DtoToTableModelService : IDtoToTableModelService
    {
        public IColumn<TData> Map<TData>(ColumnDto dto)
        {
            var columnType = Type.GetType(dto.DataType);

            var methodGeneric = this.GetType().GetMethod(nameof(Map), 2, new[] { typeof(ColumnDto) });

            var method = methodGeneric.MakeGenericMethod(typeof(TData), columnType);

            return (IColumn<TData>)method.Invoke(this, new[] { dto });
        }

        public IColumn<TData, TColumn> Map<TData, TColumn>(ColumnDto dto)
        {
            var expressionParser = new ExpressionParser(new SingleParameterTypeResolver(typeof(TData)));

            var dataGetter = expressionParser.Parse<Func<TData, TColumn>>(dto.DataGetterExpression);

            return new Column<TData, TColumn>(dto.Id, dataGetter, dto.CanSort, dto.CanFilter);
        }

        public ITableStructure<TData> Map<TData>(TableStructureDto dto)
        {
            var idType = Type.GetType(dto.IdType);

            var methodGeneric = this.GetType().GetMethod(nameof(Map), 2, new[] { typeof(TableStructureDto) });

            var method = methodGeneric.MakeGenericMethod(typeof(TData), idType);

            return (ITableStructure<TData>)method.Invoke(this, new[] { dto });
        }

        public ITableStructure<TData, TId> Map<TData, TId>(TableStructureDto dto)
        {
            var dataType = Type.GetType(dto.DataType);
            var idType = Type.GetType(dto.IdType);

            if (dataType == null)
            {
                throw new InvalidDataException($"Table structure TData mismatch {typeof(TData).FullName} [Null Dto DataType]");
            }

            if (dataType != typeof(TData))
            {
                throw new InvalidCastException($"Table structure TData mismatch {typeof(TData).FullName} [Dto {dataType.FullName}]");
            }

            if (idType == null)
            {
                throw new InvalidCastException($"Table structure TId mismatch {typeof(TId).FullName} [Null Dto IdType]");
            }

            if (idType != typeof(TId))
            {
                throw new InvalidCastException($"Table structure TId mismatch {typeof(TId).FullName} [Dto {idType.FullName}]");
            }

            return new TableStructure<TData, TId>(dto.Id, this.Map<TData, TId>(dto.IdColumn), dto.DataColumns.Select(c => this.Map<TData>(c)));
        }

        public ITableDecorator<TData, TDecorator> Map<TData, TDecorator>(TableDecoratorDto dto, ITableStructure<TData> tableStructure)
        {
            var decoratorType = Type.GetType(dto.DecoratorType);

            if (decoratorType == null)
            {
                throw new InvalidDataException($"Table decorator TDecorator mismatch {typeof(TDecorator).FullName} [Null Dto DataType]");
            }

            if (decoratorType != typeof(TDecorator))
            {
                throw new InvalidCastException($"Table decorator TDecorator mismatch {typeof(TDecorator).FullName} [Dto {decoratorType.FullName}]");
            }

            var tableDecorator = new TableDecorator<TData, TDecorator>(dto.Id, tableStructure);

            var expressionParser = new ExpressionParser(new SingleParameterTypeResolver(typeof(object)));

            var defaultDecoratorExpression = expressionParser.Parse<Func<object, TDecorator>>(dto.DefaultDecoratorExpression);

            tableDecorator.RegisterDefault(defaultDecoratorExpression);

            var methodGeneric = typeof(ColumnDecoratorMapper<TData, TDecorator>).GetMethod(
                nameof(ColumnDecoratorMapper<TData, TDecorator>.RegisterDecoratorColumn),
                1, new Type[] { typeof(ColumnDecoratorDto), typeof(TableDecorator<TData, TDecorator>), typeof(IColumn<TData>), typeof(IDtoToTableModelService) });

            if (dto.DecoratorColumns != null)
            {
                foreach (var columnDecoratorDto in dto.DecoratorColumns)
                {
                    var column = tableStructure[columnDecoratorDto.Id];
                    var method = methodGeneric.MakeGenericMethod(column.DataType);

                    method.Invoke(null, new object[] { columnDecoratorDto, tableDecorator, column, this });
                }
            }

            return tableDecorator;
        }

        public IColumnDecorator<TData, TDecorator, TColumn> Map<TData, TDecorator, TColumn>(ColumnDecoratorDto dto, IColumn<TData, TColumn> column)
        {
            var expressionParser = new ExpressionParser(new SingleParameterTypeResolver(typeof(TColumn)));

            var decoratorExpression = expressionParser.Parse<Func<TColumn, TDecorator>>(dto.DecoratorExpression);

            return new ColumnDecorator<TData, TDecorator, TColumn>(column, decoratorExpression);
        }

        private static class ColumnDecoratorMapper<TData, TDecorator>
        {
            public static void RegisterDecoratorColumn<TColumn>(ColumnDecoratorDto dto, TableDecorator<TData, TDecorator> tableDecorator, IColumn<TData> column, IDtoToTableModelService dtoToTableModelService)
            {
                var columnDecorator = dtoToTableModelService.Map<TData, TDecorator, TColumn>(dto, (IColumn<TData, TColumn>)column);
                tableDecorator.Register<TColumn>(columnDecorator);
            }
        }
    }
}
