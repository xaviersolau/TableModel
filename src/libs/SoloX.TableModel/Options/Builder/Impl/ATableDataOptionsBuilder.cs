using SoloX.TableModel.Options.Impl;

namespace SoloX.TableModel.Options.Builder.Impl
{
    public abstract class ATableDataOptionsBuilder
    {
        public string TableDataId { get; }

        protected ATableDataOptionsBuilder(string tableDataId)
        {
            TableDataId = tableDataId;
        }

        public abstract ATableDataOptions Build();
    }
}
