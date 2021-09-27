using SoloX.ExpressionTools.Transform.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    public class ColumnFilter<TData, TColumn> : IColumnFilter<TData, TColumn>
    {
        IColumn<TData> IColumnFilter<TData>.Column => Column;

        public IColumn<TData, TColumn> Column { get; }

        public Expression<Func<TData, bool>> DataFilter { get; }

        public Expression<Func<TColumn, bool>> Filter { get; }

        public ColumnFilter(IColumn<TData, TColumn> column, Expression<Func<TColumn, bool>> filter)
        {
            Column = column;

            // Value filter : (v) => 5 > v > 10
            // Column value : (d) => d.v
            // Resulting filter expression :
            // (d) => 5 > d.v > 10

            var constInliner = new ConstantInliner();
            Filter = constInliner.Amend(filter);

            var inliner = new SingleParameterInliner();
            DataFilter = inliner.Amend(column.DataGetterExpression, Filter);
        }

        public IQueryable<TData> Apply(IQueryable<TData> data)
        {
            return data.Where(DataFilter);
        }

        public void Accept(IColumnFilterVisitor<TData> visitor)
        {
            visitor.Visit(this);
        }

        public TResult Accept<TResult>(IColumnFilterVisitor<TData, TResult> visitor)
        {
            return visitor.Visit(this);
        }

        public TResult Accept<TResult, TArg>(IColumnFilterVisitor<TData, TResult, TArg> visitor, TArg arg)
        {
            return visitor.Visit(this, arg);
        }
    }
}
