using Microsoft.AspNetCore.Components;
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


            builder.Services.AddTableModel(
                tableBuilder =>
                {
                    tableBuilder
                        .UseTableStructure<WeatherForecast, DateTime>(
                            config =>
                            {
                                config
                                    .AddIdColumn(f => f.Date)
                                    .AddColumn(f => f.TemperatureC)
                                    .AddColumn(f => f.TemperatureF)
                                    .AddColumn(f => f.Summary);
                            })
                        .WithDecorator<MarkupString>("Razor",
                            config =>
                            {
                                config.AddDefault(v => new MarkupString($"<td>{v}</td>"), c => new MarkupString($"<th>{c.Id}</th>"))
                                    .Add(f => f.Date, v => new MarkupString($"<td>{v.ToShortDateString()}</td>"), () => new MarkupString($"<th>Date</th>"))
                                    .Add(f => f.TemperatureC, v => new MarkupString($"<td>{v}</td>"), () => new MarkupString($"<th>Temp. (C)</th>"))
                                    .Add(f => f.TemperatureF, v => new MarkupString($"<td>{v}</td>"), () => new MarkupString($"<th>Temp. (F)</th>"))
                                    .Add(f => f.Summary, v => new MarkupString($"<td>{v}</td>"), () => new MarkupString($"<th>Summary</th>"));
                            });

                    tableBuilder.UseRemoteTableData<WeatherForecast>(
                        config =>
                        {
                            config.HttpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/TableData/") };
                            config.HttpDataSuffix = "Data";
                            config.HttpCountSuffix = "Count";
                        });
                });

            await builder.Build().RunAsync();
        }
    }
}
