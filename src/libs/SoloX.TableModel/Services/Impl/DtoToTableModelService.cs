// ----------------------------------------------------------------------
// <copyright file="DtoToTableModelService.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.ExpressionTools.Parser.Impl;
using SoloX.ExpressionTools.Parser.Impl.Resolver;
using SoloX.ExpressionTools.Transform.Impl.Resolver;
using SoloX.TableModel.Dto;
using SoloX.TableModel.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoloX.TableModel.Services.Impl
{
    /// <summary>
    /// Dto to table model service.
    /// </summary>
    public class DtoToTableModelService : IDtoToTableModelService
    {
        private static readonly IDictionary<string, Type> BaseType = new Dictionary<string, Type>()
        {
            ["string"] = typeof(string),
            ["char"] = typeof(char),
            ["long"] = typeof(long),
            ["int"] = typeof(int),
            ["short"] = typeof(short),
            ["byte"] = typeof(byte),
            ["decimal"] = typeof(decimal),
            ["double"] = typeof(double),
            ["float"] = typeof(float),
        };

        /// <inheritdoc/>
        public IColumn<TData> Map<TData>(ColumnDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var columnType = BaseType.TryGetValue(dto.DataType, out var t) ? t : Type.GetType(dto.DataType);

            var methodGeneric = this.GetType().GetMethod(nameof(Map), 2, new[] { typeof(ColumnDto) });

            var method = methodGeneric.MakeGenericMethod(typeof(TData), columnType);

            return (IColumn<TData>)method.Invoke(this, new[] { dto });
        }

        /// <inheritdoc/>
        public static IColumn<TData, TColumn> Map<TData, TColumn>(ColumnDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var expressionParser = new ExpressionParser(new SingleParameterTypeResolver(typeof(TData)));

            var dataGetter = expressionParser.Parse<Func<TData, TColumn>>(dto.DataGetterExpression);

            var id = dto.Id ?? new PropertyNameResolver().GetPropertyName(dataGetter);

            return new Column<TData, TColumn>(id, dataGetter, dto.Header, dto.CanSort ?? true, dto.CanFilter ?? true);
        }

        /// <inheritdoc/>
        public ITableStructure<TData> Map<TData>(TableStructureDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var idType = Type.GetType(dto.IdType);

            var methodGeneric = this.GetType().GetMethod(nameof(Map), 2, new[] { typeof(TableStructureDto) });

            var method = methodGeneric.MakeGenericMethod(typeof(TData), idType);

            return (ITableStructure<TData>)method.Invoke(this, new[] { dto });
        }

        /// <inheritdoc/>
        public ITableStructure<TData, TId> Map<TData, TId>(TableStructureDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

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

            return new TableStructure<TData, TId>(dto.Id, Map<TData, TId>(dto.IdColumn), dto.DataColumns.Select(c => this.Map<TData>(c)));
        }

        /// <inheritdoc/>
        public ITableDecorator<TData, TDecorator> Map<TData, TDecorator>(TableDecoratorDto dto, ITableStructure<TData> tableStructure)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

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

            var headerExpressionParser = new ExpressionParser(new SingleParameterTypeResolver(typeof(IColumn<TData>)));

            var defaultHeaderDecoratorExpression = headerExpressionParser.Parse<Func<IColumn<TData>, TDecorator>>(dto.DefaultHeaderDecoratorExpression);

            tableDecorator.RegisterDefault(defaultDecoratorExpression, defaultHeaderDecoratorExpression);

            var methodGeneric = typeof(ColumnDecoratorMapper<TData, TDecorator>).GetMethod(
                nameof(ColumnDecoratorMapper<TData, TDecorator>.RegisterDecoratorColumn),
                1, new Type[] { typeof(ColumnDecoratorDto), typeof(TableDecorator<TData, TDecorator>), typeof(IColumn<TData>), typeof(IDtoToTableModelService) });

            if (dto.DecoratorColumns != null)
            {
                foreach (var columnDecoratorDto in dto.DecoratorColumns)
                {
                    var column = tableStructure[columnDecoratorDto.Id];
                    var method = methodGeneric.MakeGenericMethod(column.DataType);

                    var registered = (bool)method.Invoke(null, new object[] { columnDecoratorDto, tableDecorator, column, this });

                    // TODO log warning if not registered.
                }
            }

            return tableDecorator;
        }

        /// <inheritdoc/>
        public IColumnDecorator<TData, TDecorator, TColumn> Map<TData, TDecorator, TColumn>(ColumnDecoratorDto dto, IColumn<TData, TColumn> column)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var expressionParser = new ExpressionParser(new SingleParameterTypeResolver(typeof(TColumn)));

            var decoratorExpression = expressionParser.Parse<Func<TColumn, TDecorator>>(dto.DecoratorExpression);

            var headerExpressionParser = new ExpressionParser();

            var headerDecoratorExpression = headerExpressionParser.Parse<Func<TDecorator>>(dto.HeaderDecoratorExpression);

            return new ColumnDecorator<TData, TDecorator, TColumn>(column, decoratorExpression, headerDecoratorExpression);
        }

        private static class ColumnDecoratorMapper<TData, TDecorator>
        {
            public static bool RegisterDecoratorColumn<TColumn>(ColumnDecoratorDto dto, TableDecorator<TData, TDecorator> tableDecorator, IColumn<TData> column, IDtoToTableModelService dtoToTableModelService)
            {
                var columnDecorator = dtoToTableModelService.Map<TData, TDecorator, TColumn>(dto, (IColumn<TData, TColumn>)column);
                return tableDecorator.TryRegister<TColumn>(columnDecorator);
            }
        }
    }
}
