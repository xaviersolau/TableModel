using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    public class TableFactory : ITableFactory
    {
        public ITableFilter<TData> CreateTableFilter<TData>()
        {
            return new TableFilter<TData>();
        }

        public ITableSorting<TData> CreateTableSorting<TData>()
        {
            return new TableSorting<TData>();
        }
    }
}
