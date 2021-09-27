using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface IColumnVisitor<TData>
    {
        void Visit<TColumn>(IColumn<TData, TColumn> column);
    }

    public interface IColumnVisitor<TData, TResult>
    {
        TResult Visit<TColumn>(IColumn<TData, TColumn> column);
    }
    public interface IColumnVisitor<TData, TResult, TArg>
    {
        TResult Visit<TColumn>(IColumn<TData, TColumn> column, TArg arg);
    }
}
