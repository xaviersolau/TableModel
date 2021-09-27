using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Dto
{
    public class ColumnDto
    {
        public string Id { get; set; }

        public string DataType { get; set; }

        public bool CanSort { get; set; }

        public bool CanFilter { get; set; }

        public string DataGetterExpression { get; set; }
    }
}
