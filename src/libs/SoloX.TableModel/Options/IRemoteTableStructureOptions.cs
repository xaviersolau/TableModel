using System.Net.Http;

namespace SoloX.TableModel.Options
{
    public interface IRemoteTableStructureOptions<TData, TId>
    {
        HttpClient HttpClient { get; set; }
    }
}
