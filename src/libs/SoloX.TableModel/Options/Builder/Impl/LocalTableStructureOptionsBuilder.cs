using SoloX.TableModel.Options;
using System;
using System.Collections.Generic;
using SoloX.TableModel.Options.Builder;
using SoloX.TableModel.Options.Impl;

namespace SoloX.TableModel.Options.Builder.Impl
{
    public class LocalTableStructureOptionsBuilder<TData, TId> : ATableStructureOptionsBuilder, ILocalTableStructureOptionsBuilder<TData, TId>
    {
        private readonly Action<ILocalTableStructureOptions<TData, TId>> tableStructureOptionsSetup;

        private readonly List<ATableDecoratorOptions> tableDecoratorOptions = new List<ATableDecoratorOptions>();

        public string TableStructureId { get; }
        public IEnumerable<ATableDecoratorOptions> TableDecoratorOptions => tableDecoratorOptions;

        public LocalTableStructureOptionsBuilder(string tableStructureId, Action<ILocalTableStructureOptions<TData, TId>> tableStructureOptionsSetup)
        {
            TableStructureId = tableStructureId;
            this.tableStructureOptionsSetup = tableStructureOptionsSetup;
        }

        public override ATableStructureOptions Build()
        {
            var opt = new LocalTableStructureOptions<TData, TId>(TableStructureId, tableDecoratorOptions);

            tableStructureOptionsSetup(opt);

            return opt;
        }

        public ILocalTableStructureOptionsBuilder<TData, TId> WithDecorator<TDecorator>(string decoratorId, Action<ILocalTableDecoratorOptions<TData, TDecorator>> tableDecoratorOptionsSetup)
        {
            var decoratorOptions = new LocalTableDecoratorOptions<TData, TId, TDecorator>(TableStructureId, decoratorId);

            tableDecoratorOptionsSetup(decoratorOptions);

            tableDecoratorOptions.Add(decoratorOptions);

            return this;
        }
    }
}
