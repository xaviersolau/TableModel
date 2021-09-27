using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface ITableDataVisitor
    {
        void Visit<TData>(ITableData<TData> tableData);
    }

    public interface ITableDataVisitor<TResult>
    {
        TResult Visit<TData>(ITableData<TData> tableData);
    }

    public interface ITableDataVisitor<TResult, TArg>
    {
        TResult Visit<TData>(ITableData<TData> tableData, TArg arg);
    }
}
