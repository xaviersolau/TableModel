using System;
using System.Linq.Expressions;

namespace SoloX.TableModel.Options
{
    public interface ILocalTableDecoratorDataOptions<TData, TDecorator>
    {
        ILocalTableDecoratorDataOptions<TData, TDecorator> Add<TColumn>(string columnId, Expression<Func<TColumn, TDecorator>> decoratorExpression);
    }
}
