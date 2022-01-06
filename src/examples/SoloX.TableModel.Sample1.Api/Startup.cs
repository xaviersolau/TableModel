using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SoloX.TableModel.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoloX.TableModel.Sample1.Api
{
    public class Startup
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SoloX.TableModel.Sample1.Api", Version = "v1" });
            });

            services.AddTableModelServer(
                builder =>
                {
                    builder.UseMemoryTableData<WeatherForecast>(
                        config =>
                        {
                            var rng = new Random();
                            var data = Enumerable.Range(1, 50).Select(index => new WeatherForecast
                            {
                                Date = DateTime.Now.AddDays(index),
                                TemperatureC = rng.Next(-20, 55),
                                Summary = Summaries[rng.Next(Summaries.Length)]
                            })
                            .ToArray();

                            config.AddData(data);
                        });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SoloX.TableModel.Sample1.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
