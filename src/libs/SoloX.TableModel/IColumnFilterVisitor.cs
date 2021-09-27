using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface IColumnFilterVisitor<TData>
    {
        void Visit<TColumn>(IColumnFilter<TData, TColumn> columnFilter);
    }

    public interface IColumnFilterVisitor<TData, TResult>
    {
        TResult Visit<TColumn>(IColumnFilter<TData, TColumn> columnFilter);
    }

    public interface IColumnFilterVisitor<TData, TResult, TArg>
    {
        TResult Visit<TColumn>(IColumnFilter<TData, TColumn> columnFilter, TArg args);
    }
}
