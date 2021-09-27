using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Dto
{
    public class DataRequestDto
    {
        public int? Offset { get; set; }

        public int? PageSize { get; set; }

        public IEnumerable<SortingDto> Sortings { get; set; }
        public IEnumerable<FilterDto> Filters { get; set; }
    }
}
