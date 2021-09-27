using System;
using System.Linq.Expressions;
using SoloX.ExpressionTools.Transform.Impl;

namespace SoloX.TableModel.Impl
{
    public class ColumnDecorator<TData, TDecorator, TColumn> : IColumnDecorator<TData, TDecorator, TColumn>
    {
        private readonly Func<TData, TDecorator> decorator;

        public ColumnDecorator(IColumn<TData, TColumn> column,
            Expression<Func<TColumn, TDecorator>> relativeDecoratorExpression)
        {
            Column = column;
            RelativeDecoratorExpression = relativeDecoratorExpression;

            // Value filter : (v) => v.ToString()
            // Column value : (d) => d.v
            // Resulting filter expression :
            // (d) => d.v.ToString()

            var inliner = new SingleParameterInliner();
            var absoluteDecoratorExpression = inliner.Amend(column.DataGetterExpression, relativeDecoratorExpression);

            AbsoluteDecoratorExpression = absoluteDecoratorExpression;

            decorator = absoluteDecoratorExpression.Compile();
        }

        public IColumn<TData, TColumn> Column { get; }

        IColumn<TData> IColumnDecorator<TData, TDecorator>.Column => Column;

        public Expression<Func<TColumn, TDecorator>> RelativeDecoratorExpression { get; }

        public Expression<Func<TData, TDecorator>> AbsoluteDecoratorExpression { get; }

        public TDecorator Decorate(TData data)
        {
            return decorator(data);
        }

        public void Accept(IColumnDecoratorVisitor<TData, TDecorator> visitor)
        {
            visitor.Visit(this);
        }

        public TResult Accept<TResult>(IColumnDecoratorVisitor<TData, TDecorator, TResult> visitor)
        {
            return visitor.Visit(this);
        }

        public TResult Accept<TResult, TArg>(IColumnDecoratorVisitor<TData, TDecorator, TResult, TArg> visitor, TArg arg)
        {
            return visitor.Visit(this, arg);
        }
    }
}