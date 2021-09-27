using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoloX.TableModel.Options.Impl
{
    public abstract class ATableStructureOptions
    {
        protected ATableStructureOptions(string tableStructureId, List<ATableDecoratorOptions> tableDecoratorOptions)
        {
            TableStructureId = tableStructureId;
            TableDecoratorOptions = tableDecoratorOptions;
        }

        public string TableStructureId { get; }
        
        public IEnumerable<ATableDecoratorOptions> TableDecoratorOptions { get; }

        public abstract Task<ITableStructure> CreateModelInstanceAsync(IServiceProvider serviceProvider);
    }
}
