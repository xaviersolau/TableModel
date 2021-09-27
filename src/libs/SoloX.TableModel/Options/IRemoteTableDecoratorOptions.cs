using System.Net.Http;

namespace SoloX.TableModel.Options
{
    public interface IRemoteTableDecoratorOptions<TData, TDecorator>
    {
        HttpClient HttpClient { get; set; }
    }
}
