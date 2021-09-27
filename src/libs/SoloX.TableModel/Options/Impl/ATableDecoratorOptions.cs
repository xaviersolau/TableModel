using System;
using System.Threading.Tasks;

namespace SoloX.TableModel.Options.Impl
{
    public abstract class ATableDecoratorOptions
    {
        public string TableDecoratorId { get; }
        public string TableStructureId { get; }
        protected ATableDecoratorOptions(string tableStructureId, string tableDecoratorId)
        {
            TableStructureId = tableStructureId;
            TableDecoratorId = tableDecoratorId;
        }

        public abstract Task<ITableDecorator> CreateModelInstanceAsync(IServiceProvider serviceProvider, ITableStructureRepository tableStructureRepository);
    }
}
