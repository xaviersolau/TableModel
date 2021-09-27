using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel
{
    public interface ITableStructure
    {
        string Id { get; }
    }

    public interface ITableStructure<TData> : ITableStructure
    {
        IColumn<TData> this[string id] { get; }

        IEnumerable<IColumn<TData>> Columns { get; }
    }

    public interface ITableStructure<TData, TId> : ITableStructure<TData>
    {
        IColumn<TData, TId> IdColumn { get; }

        IEnumerable<IColumn<TData>> DataColumns { get; }
    }
}
