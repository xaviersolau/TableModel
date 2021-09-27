using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface IColumnFilter<TData>
    {
        IColumn<TData> Column { get; }

        IQueryable<TData> Apply(IQueryable<TData> data);

        Expression<Func<TData, bool>> DataFilter { get; }

        void Accept(IColumnFilterVisitor<TData> visitor);
        TResult Accept<TResult>(IColumnFilterVisitor<TData, TResult> visitor);
        TResult Accept<TResult, TArg>(IColumnFilterVisitor<TData, TResult, TArg> visitor, TArg arg);
    }

    public interface IColumnFilter<TData, TColumn> : IColumnFilter<TData>
    {
        new IColumn<TData, TColumn> Column { get; }

        Expression<Func<TColumn, bool>> Filter { get; }
    }
}
