using SoloX.TableModel.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SoloX.TableModel.Services;
using SoloX.TableModel.Dto;
using System.Net.Http.Json;

namespace SoloX.TableModel.Options.Impl
{
    public class RemoteTableDecoratorOptions<TData, TId, TDecorator> : ATableDecoratorOptions, IRemoteTableDecoratorOptions<TData, TDecorator>
    {
        public HttpClient HttpClient { get; set; }

        public RemoteTableDecoratorOptions(string tableStructureId, string tableDecoratorId)
            : base(tableStructureId, tableDecoratorId)
        {
        }

        public override async Task<ITableDecorator> CreateModelInstanceAsync(IServiceProvider serviceProvider, ITableStructureRepository tableStructureRepository)
        {
            var tableStructure = await tableStructureRepository.GetTableStructureAsync<TData>(TableStructureId);

            var dtoToTableModelService = serviceProvider.GetRequiredService<IDtoToTableModelService>();

            var tableDecoratorDto = await HttpClient.GetFromJsonAsync<TableDecoratorDto>($"{TableStructureId}/{TableDecoratorId}");

            return dtoToTableModelService.Map<TData, TDecorator>(tableDecoratorDto, tableStructure);
        }
    }
}
