using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Dto
{
    public class SortingDto
    {
        public ColumnDto Column { get; set; }

        public SortingOrder Order { get; set; }
    }
}
