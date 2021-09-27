using System;
using System.Linq.Expressions;

namespace SoloX.TableModel.Options
{
    public interface ILocalTableStructureOptions<TData, TId>
    {
        ILocalTableStructureDataOptions<TData, TId> AddIdColumn(string columnId, Expression<Func<TData, TId>> idGetterExpression, bool canSort = true, bool canFilter = true);
    }
}
