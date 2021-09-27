using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    public class ColumnSorting<TData, TColumn> : IColumnSorting<TData>
    {
        public IColumn<TData> Column { get; }

        public SortingOrder Order { get; }

        private Expression<Func<TData, TColumn>> DataGetter { get; }

        public ColumnSorting(IColumn<TData, TColumn> column, SortingOrder order)
        {
            Column = column;
            Order = order;
            DataGetter = column.DataGetterExpression;
        }

        public IQueryable<TData> Apply(IQueryable<TData> data)
        {
            return Order == SortingOrder.Ascending
                ? data.OrderBy(DataGetter)
                : data.OrderByDescending(DataGetter);
        }
    }
}
