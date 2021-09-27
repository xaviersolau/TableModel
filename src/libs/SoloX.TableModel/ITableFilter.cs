using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface ITableFilter<TData>
    {
        IEnumerable<IColumnFilter<TData>> ColumnFilters { get; }

        void Register<TColumn>(Expression<Func<TData, TColumn>> data, Expression<Func<TColumn, bool>> filter);
        void Register<TColumn>(string columnId, Expression<Func<TData, TColumn>> data, Expression<Func<TColumn, bool>> filter);
        void Register<TColumn>(IColumn<TData> column, Expression<Func<TColumn, bool>> filter);
        void Register<TColumn>(IColumn<TData, TColumn> column, Expression<Func<TColumn, bool>> filter);

        void UnRegister(IColumn<TData> column);
        void UnRegister(string columnId);

        IQueryable<TData> Apply(IQueryable<TData> data);
    }
}
