using SoloX.TableModel.Options;
using System;
using System.Collections.Generic;
using SoloX.TableModel.Options.Builder;
using SoloX.TableModel.Options.Impl;

namespace SoloX.TableModel.Options.Builder.Impl
{
    public class RemoteTableStructureOptionsBuilder<TData, TId> : ATableStructureOptionsBuilder, IRemoteTableStructureOptionsBuilder<TData, TId>
    {
        private readonly Action<IRemoteTableStructureOptions<TData, TId>> tableStructureOptionsSetup;

        private readonly List<ATableDecoratorOptions> tableDecoratorOptions = new List<ATableDecoratorOptions>();

        public string TableStructureId { get; }
        public IEnumerable<ATableDecoratorOptions> TableDecoratorOptions => tableDecoratorOptions;

        public RemoteTableStructureOptionsBuilder(string tableStructureId, Action<IRemoteTableStructureOptions<TData, TId>> tableStructureOptionsSetup)
        {
            TableStructureId = tableStructureId;
            this.tableStructureOptionsSetup = tableStructureOptionsSetup;
        }

        public override ATableStructureOptions Build()
        {
            var opt = new RemoteTableStructureOptions<TData, TId>(TableStructureId, tableDecoratorOptions);

            tableStructureOptionsSetup(opt);

            return opt;
        }

        public IRemoteTableStructureOptionsBuilder<TData, TId> WithRemoteDecorator<TDecorator>(string decoratorId, Action<IRemoteTableDecoratorOptions<TData, TDecorator>> tableDecoratorOptionsSetup)
        {
            var decoratorOptions = new RemoteTableDecoratorOptions<TData, TId, TDecorator>(TableStructureId, decoratorId);

            tableDecoratorOptionsSetup(decoratorOptions);

            tableDecoratorOptions.Add(decoratorOptions);

            return this;
        }

        public IRemoteTableStructureOptionsBuilder<TData, TId> WithDecorator<TDecorator>(string decoratorId, Action<ILocalTableDecoratorOptions<TData, TDecorator>> tableDecoratorOptionsSetup)
        {
            var decoratorOptions = new LocalTableDecoratorOptions<TData, TId, TDecorator>(TableStructureId, decoratorId);

            tableDecoratorOptionsSetup(decoratorOptions);

            tableDecoratorOptions.Add(decoratorOptions);

            return this;
        }
    }
}
