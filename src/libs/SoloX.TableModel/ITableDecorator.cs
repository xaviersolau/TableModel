using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface ITableDecorator
    {
        string Id { get; }

        Type DecoratorType { get; }
    }

    public interface ITableDecorator<TData, TDecorator> : ITableDecorator
    {
        ITableStructure<TData> TableStructure { get; }

        Expression<Func<object, TDecorator>> DefaultDecoratorExpression { get; }

        IEnumerable<IColumnDecorator<TData, TDecorator>> TableColumnDecorators { get; }

        TDecorator Decorate(IColumn<TData> tableColumn, TData data);
    }
}
