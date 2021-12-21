using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoloX.TableModel.Sample2.Shared;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Sample2.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


            builder.Services.AddTableStructure(
                tableBuilder =>
                {
                    tableBuilder.UseRemoteTableData<WeatherForecast>(
                        "WeatherForecast",
                        config =>
                        {
                            config.HttpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/TableModel/") };
                        });
                });

            await builder.Build().RunAsync();
        }
    }
}