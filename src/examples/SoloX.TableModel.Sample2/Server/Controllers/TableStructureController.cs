using Microsoft.AspNetCore.Mvc;
using SoloX.TableModel.Server;
using SoloX.TableModel.Server.Services;

namespace SoloX.TableModel.Sample2.Server.Controllers
{
    /// <summary>
    /// The TableStructureController based on TableStructureControllerBase that is providing the end-points to
    /// get table structure definition.
    /// </summary>
    /// <remarks>
    /// You can also use the Authorization attributes here.
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class TableStructureController : TableStructureControllerBase
    {
        /// <summary>
        /// Setup the TableStructureController with the tableStructureEndPointService provided in the
        /// SoloX.TableModel.Server package.
        /// </summary>
        /// <param name="tableStructureEndPointService">The end-point service that is actually doing the job.</param>
        public TableStructureController(ITableStructureEndPointService tableStructureEndPointService)
            : base(tableStructureEndPointService)
        {
        }
    }
}
