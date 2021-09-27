using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Dto
{
    public class FilterDto
    {
        public ColumnDto Column { get; set; }

        public string FilterExpression { get; set; }
    }
}
