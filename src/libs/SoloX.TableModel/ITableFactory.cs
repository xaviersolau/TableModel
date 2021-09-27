using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface ITableFactory
    {
        ITableFilter<TData> CreateTableFilter<TData>();

        ITableSorting<TData> CreateTableSorting<TData>();
    }
}
