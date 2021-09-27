using SoloX.TableModel.Options;
using SoloX.TableModel.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SoloX.TableModel.Dto;
using System.Net.Http.Json;

namespace SoloX.TableModel.Options.Impl
{
    public class RemoteTableStructureOptions<TData, TId> : ATableStructureOptions, IRemoteTableStructureOptions<TData, TId>
    {
        public RemoteTableStructureOptions(string tableStructureId, List<ATableDecoratorOptions> tableDecoratorOptions)
            : base(tableStructureId, tableDecoratorOptions)
        { }

        public HttpClient HttpClient { get; set; }

        public override async Task<ITableStructure> CreateModelInstanceAsync(IServiceProvider serviceProvider)
        {
            var dtoToTableModelService = serviceProvider.GetRequiredService<IDtoToTableModelService>();
            
            var tableStructureDto = await HttpClient.GetFromJsonAsync<TableStructureDto>(TableStructureId);

            return dtoToTableModelService.Map<TData, TId>(tableStructureDto);
        }
    }
}
