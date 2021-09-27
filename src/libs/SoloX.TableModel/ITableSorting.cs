using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface ITableSorting<TData>
    {
        IEnumerable<IColumnSorting<TData>> ColumnSortings { get; }

        void Register<TColumn>(Expression<Func<TData, TColumn>> data, SortingOrder order);

        void Register<TColumn>(string columnId, Expression<Func<TData, TColumn>> data, SortingOrder order);

        void Register(IColumn<TData> column, SortingOrder order);

        void UnRegister(IColumn<TData> column);

        IQueryable<TData> Apply(IQueryable<TData> data);
    }
}
