using System.Net.Http;

namespace SoloX.TableModel.Options
{
    public interface IRemoteTableDataOptions<TData>
    {
        HttpClient HttpClient { get; set; }
    }
}
