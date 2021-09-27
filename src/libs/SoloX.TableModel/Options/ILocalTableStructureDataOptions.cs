using System;
using System.Linq.Expressions;

namespace SoloX.TableModel.Options
{
    public interface ILocalTableStructureDataOptions<TData, TId>
    {
        ILocalTableStructureDataOptions<TData, TId> AddColumn<TColumn>(string columnId, Expression<Func<TData, TColumn>> dataGetterExpression, bool canSort = true, bool canFilter = true);
    }
}
