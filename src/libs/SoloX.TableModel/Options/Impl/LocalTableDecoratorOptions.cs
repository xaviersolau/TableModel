using SoloX.TableModel.Impl;
using SoloX.TableModel.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SoloX.TableModel.Options.Impl
{
    public class LocalTableDecoratorOptions<TData, TId, TDecorator> : ATableDecoratorOptions, ILocalTableDecoratorOptions<TData, TDecorator>, ILocalTableDecoratorDataOptions<TData, TDecorator>
    {
        public Expression<Func<object, TDecorator>> DefaultDecoratorExpression { get; private set; }

        public IReadOnlyDictionary<string, Action<TableDecorator<TData, TDecorator>>> ColumnDecoratorRegisterActions => columnDecoratorRegisterActions;

        private Dictionary<string, Action<TableDecorator<TData, TDecorator>>> columnDecoratorRegisterActions = new Dictionary<string, Action<TableDecorator<TData, TDecorator>>>();

        public LocalTableDecoratorOptions(string tableStructureId, string tableDecoratorId)
            : base(tableStructureId, tableDecoratorId)
        {
        }

        public ILocalTableDecoratorDataOptions<TData, TDecorator> AddDefault(Expression<Func<object, TDecorator>> defaultDecoratorExpression)
        {
            if (DefaultDecoratorExpression != null)
            {
                throw new InvalidDataException("Default decorator expression already defined");
            }

            DefaultDecoratorExpression = defaultDecoratorExpression;

            return this;
        }

        public ILocalTableDecoratorDataOptions<TData, TDecorator> Add<TColumn>(string columnId, Expression<Func<TColumn, TDecorator>> decoratorExpression)
        {
            if (columnDecoratorRegisterActions.ContainsKey(columnId))
            {
                throw new InvalidDataException($"Decorator expression already defined fro {columnId}");
            }

            columnDecoratorRegisterActions.Add(columnId, tableDecorator => {
                tableDecorator.Register(columnId, decoratorExpression);
            });

            return this;
        }

        public override async Task<ITableDecorator> CreateModelInstanceAsync(IServiceProvider serviceProvider, ITableStructureRepository tableStructureRepository)
        {
            var tableStructure = await tableStructureRepository.GetTableStructureAsync<TData>(TableStructureId);
            var tableDecorator = new TableDecorator<TData, TDecorator>(TableDecoratorId, tableStructure);

            tableDecorator.RegisterDefault(DefaultDecoratorExpression);

            foreach (var setupAction in ColumnDecoratorRegisterActions)
            {
                setupAction.Value(tableDecorator);
            }

            return tableDecorator;
        }
    }
}
