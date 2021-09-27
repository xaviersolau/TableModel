using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Dto
{
    public class TableStructureDto
    {
        public string Id { get; set; }

        public string DataType { get; set; }

        public string IdType { get; set; }

        public ColumnDto IdColumn { get; set; }

        public IEnumerable<ColumnDto> DataColumns { get; set; }
    }
}
