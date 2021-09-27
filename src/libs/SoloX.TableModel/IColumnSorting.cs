using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface IColumnSorting<TData>
    {
        IColumn<TData> Column { get; }

        SortingOrder Order { get; }

        IQueryable<TData> Apply(IQueryable<TData> data);
    }
}
