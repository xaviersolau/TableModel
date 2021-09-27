using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Options.Builder
{
    public interface IRemoteTableStructureOptionsBuilder<TData, TId>
    {
        IRemoteTableStructureOptionsBuilder<TData, TId> WithDecorator<TDecorator>(string decoratorId, Action<ILocalTableDecoratorOptions<TData, TDecorator>> tableDecoratorOptionsSetup);
        IRemoteTableStructureOptionsBuilder<TData, TId> WithRemoteDecorator<TDecorator>(string decoratorId, Action<IRemoteTableDecoratorOptions<TData, TDecorator>> tableDecoratorOptionsSetup);
    }
}
