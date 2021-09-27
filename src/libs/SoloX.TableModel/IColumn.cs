using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface IColumn<TData>
    {
        string Id { get; }
        bool CanSort { get; }
        bool CanFilter { get; }
        Type DataType { get; }
        object GetObject(TData data);

        void Accept(IColumnVisitor<TData> visitor);
        TResult Accept<TResult>(IColumnVisitor<TData, TResult> visitor);
        TResult Accept<TResult, TArg>(IColumnVisitor<TData, TResult, TArg> visitor, TArg arg);
    }

    public interface IColumn<TData, TColumn> : IColumn<TData>
    {
        Expression<Func<TData, TColumn>> DataGetterExpression { get; }
        TColumn GetValue(TData data);
    }
}
