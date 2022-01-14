using Microsoft.AspNetCore.Mvc;
using SoloX.TableModel.Server;
using SoloX.TableModel.Server.Services;

namespace SoloX.TableModel.Sample2.Server.Controllers
{
    /// <summary>
    /// The TableDataController based on TableDataControllerBase that is providing the end-points to
    /// query table data.
    /// </summary>
    /// <remarks>
    /// You can also use the Authorization attributes here.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class TableDataController : TableDataControllerBase
    {
        /// <summary>
        /// Setup the TableDataController with the tableDataEndPointService provided in the
        /// SoloX.TableModel.Server package.
        /// </summary>
        /// <param name="tableStructureEndPointService">The end-point service that is actually doing the job.</param>
        public TableDataController(ITableDataEndPointService tableDataEndPointService)
            : base(tableDataEndPointService)
        {
            EnableAsyncEnumerableDataStreaming = true;
        }
    }
}
