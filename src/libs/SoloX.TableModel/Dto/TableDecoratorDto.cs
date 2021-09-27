using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Dto
{
    public class TableDecoratorDto
    {
        public string Id { get; set; }

        public string DecoratorType { get; set; }

        public string DefaultDecoratorExpression { get; set; }

        public IEnumerable<ColumnDecoratorDto> DecoratorColumns { get; set; }
    }
}
