using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface IColumnDecorator<TData, TDecorator>
    {
        IColumn<TData> Column { get; }

        Expression<Func<TData, TDecorator>> AbsoluteDecoratorExpression { get; }

        TDecorator Decorate(TData data);

        void Accept(IColumnDecoratorVisitor<TData, TDecorator> visitor);
        TResult Accept<TResult>(IColumnDecoratorVisitor<TData, TDecorator, TResult> visitor);
        TResult Accept<TResult, TArg>(IColumnDecoratorVisitor<TData, TDecorator, TResult, TArg> visitor, TArg arg);
    }

    public interface IColumnDecorator<TData, TDecorator, TColumn> : IColumnDecorator<TData, TDecorator>
    {
        new IColumn<TData, TColumn> Column { get; }

        Expression<Func<TColumn, TDecorator>> RelativeDecoratorExpression { get; }
    }
}
