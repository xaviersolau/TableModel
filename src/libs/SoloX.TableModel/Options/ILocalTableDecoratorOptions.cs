using System;
using System.Linq.Expressions;

namespace SoloX.TableModel.Options
{
    public interface ILocalTableDecoratorOptions<TData, TDecorator>
    {
        ILocalTableDecoratorDataOptions<TData, TDecorator> AddDefault(Expression<Func<object, TDecorator>> defaultDecoratorExpression);
    }
}
