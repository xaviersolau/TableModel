using System.Collections.Generic;

namespace SoloX.TableModel.Options.Impl
{
    public class TableModelOptions
    {
        public IEnumerable<ATableStructureOptions> TableStructureOptions { get; internal set; }
        public IEnumerable<ATableDataOptions> TableDataOptions { get; internal set; }
    }
}
