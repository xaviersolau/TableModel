using Microsoft.Extensions.DependencyInjection;
using SoloX.TableModel.Impl;
using System;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using SoloX.TableModel.Dto;
using SoloX.TableModel.Services.Impl;
using SoloX.TableModel.Options.Builder;
using SoloX.TableModel.Options.Builder.Impl;
using SoloX.TableModel.Options.Impl;
using SoloX.TableModel.Services;

namespace SoloX.TableModel
{
    public static class TableModelExtensions
    {
        public static void AddTableStructure(this IServiceCollection services, Action<ITableModelOptionsBuilder> setupAction)
        {
            services.AddScoped<ITableDataRepository, TableDataRepository>();
            services.AddScoped<ITableStructureRepository, TableStructureRepository>();

            services.AddSingleton<IDtoToTableModelService, DtoToTableModelService>();

            var tableModelOptionsBuilder = new TableModelOptionsBuilder(setupAction);

            services.Configure<TableModelOptions>(tableModelOptionsBuilder.Build);
        }
    }
}
