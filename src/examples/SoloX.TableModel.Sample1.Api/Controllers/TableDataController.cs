using Microsoft.AspNetCore.Mvc;
using SoloX.TableModel.Server;
using SoloX.TableModel.Server.Services;

namespace SoloX.TableModel.Sample1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableDataController : TableDataControllerBase
    {
        public TableDataController(ITableDataEndPointService tableDataEndPointService)
            : base(tableDataEndPointService)
        {
            EnableAsyncEnumerableDataStreaming = true;
        }
    }
}
