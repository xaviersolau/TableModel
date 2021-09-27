using SoloX.ExpressionTools.Transform.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    public class TableDecorator<TData, TDecorator> : ITableDecorator<TData, TDecorator>
    {
        private readonly Dictionary<string, IColumnDecorator<TData, TDecorator>> decoratorExpression = new Dictionary<string, IColumnDecorator<TData, TDecorator>>();

        private Func<object, TDecorator> defaultDecorator;

        public Expression<Func<object, TDecorator>> DefaultDecoratorExpression { get; private set; }

        public IEnumerable<IColumnDecorator<TData, TDecorator>> TableColumnDecorators => this.decoratorExpression.Values;

        public ITableStructure<TData> TableStructure { get; }

        public string Id { get; }

        public Type DecoratorType => typeof(TDecorator);

        public TableDecorator(string id, ITableStructure<TData> tableStructure)
        {
            TableStructure = tableStructure;
            Id = id;
        }

        public void RegisterDefault(Expression<Func<object, TDecorator>> decoratorExpression)
        {

            DefaultDecoratorExpression = decoratorExpression;
            this.defaultDecorator = decoratorExpression.Compile();
        }

        public void Register<TColumn>(string columnId, Expression<Func<TColumn, TDecorator>> decoratorExpression)
        {
            var column = TableStructure[columnId];

            if (column is IColumn<TData, TColumn> typedColumn)
            {
                Register(typedColumn, decoratorExpression);
            }
            else
            {
                throw new ArgumentException($"Incompatible column type {column.Id} [Type: {typeof(TColumn).Name}]");
            }
        }

        private void Register<TColumn>(IColumn<TData, TColumn> tableColumn, Expression<Func<TColumn, TDecorator>> relativeDecoratorExpression)
        {

            Register(new ColumnDecorator<TData, TDecorator, TColumn>(tableColumn, relativeDecoratorExpression));
        }

        public void Register<TColumn>(IColumnDecorator<TData, TDecorator, TColumn> tableColumnDecorator)
        {
            var matchingColumn = TableStructure.Columns.FirstOrDefault(x => x.Id == tableColumnDecorator.Column.Id);

            if (matchingColumn == null)
            {
                throw new InvalidDataException(
                    $"Table column decorator {tableColumnDecorator.Column.Id} is not compatible with the current table structure {TableStructure.Id}.");
            }

            this.decoratorExpression.Add(tableColumnDecorator.Column.Id, tableColumnDecorator);
        }

        public TDecorator Decorate(IColumn<TData> tableColumn, TData data)
        {
            if (this.decoratorExpression.TryGetValue(tableColumn.Id, out var columnDecorator))
            {
                return columnDecorator.Decorate(data);
            }

            return this.defaultDecorator != null ? this.defaultDecorator(tableColumn.GetObject(data)) : default;
        }

    }
}
