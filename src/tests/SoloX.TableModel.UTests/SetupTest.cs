using Microsoft.Extensions.DependencyInjection;
using SoloX.TableModel;
using SoloX.TableModel.UTests.Samples;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Xunit;
using SoloX.CodeQuality.Test.Helpers.Http;
using SoloX.TableModel.Dto;

namespace SoloX.TableModel.UTests
{
    public class SetupTest
    {
        [Fact]
        public async Task ItShouldSetupServicesFromLocal()
        {

            var services = new ServiceCollection();

            services.AddTableStructure(
                builder =>
                {
                    builder
                        .UseTableStructure<Person, int>(
                            "LocalPersonTable",
                            config =>
                            {
                                config
                                    .AddIdColumn(nameof(Person.Id), p => p.Id)
                                    .AddColumn(nameof(Person.FirstName), p => p.FirstName)
                                    .AddColumn(nameof(Person.LastName), p => p.LastName)
                                    .AddColumn(nameof(Person.Email), p => p.Email)
                                    .AddColumn(nameof(Person.BirthDate), p => p.BirthDate);
                            })
                        .WithDecorator<string>(
                            "TextDecorator1",
                             config =>
                             {
                                 config
                                     .AddDefault(v => v.ToString())
                                     .Add<string>(nameof(Person.LastName), n => n.ToUpper())
                                     .Add<DateTime>(nameof(Person.BirthDate), date => date.ToString("D", CultureInfo.InvariantCulture));
                             })
                        .WithDecorator<string>(
                            "TextDecorator2",
                             config =>
                             {
                                 config
                                     .AddDefault(v => v.ToString())
                                     .Add<string>(nameof(Person.LastName), n => n.ToLower());
                             });

                    builder.UseMemoryTableData<Person>(
                        "MemoryPersonTableData",
                        config =>
                        {
                            config.AddData(Person.GetSomePersons());
                        });
                });

            using var sp = services.BuildServiceProvider();

            var tableStructureRepository = sp.GetService<ITableStructureRepository>();

            Assert.NotNull(tableStructureRepository);

            var tableStructure = await tableStructureRepository.GetTableStructureAsync<Person, int>("LocalPersonTable");

            Assert.NotNull(tableStructure);

            var tableDecorator1 = await tableStructureRepository.GetTableDecoratorAsync<Person, string>("LocalPersonTable", "TextDecorator1");

            Assert.NotNull(tableDecorator1);

            var tableDecorator2 = await tableStructureRepository.GetTableDecoratorAsync<Person, string>("LocalPersonTable", "TextDecorator2");

            Assert.NotNull(tableDecorator2);

            var tableDataRepository = sp.GetService<ITableDataRepository>();

            Assert.NotNull(tableDataRepository);

            var tableData = await tableDataRepository.GetTableDataAsync<Person>("MemoryPersonTableData");

            Assert.NotNull(tableData);
        }

        [Fact]
        public async Task ItShouldSetupServicesFromRemote()
        {
            var baseAddress = "http://host/api/TableModel";
            var httpClientBuilder = new HttpClientMockBuilder();

            var httpClient = httpClientBuilder
                .WithBaseAddress(new Uri(baseAddress))
                .WithRequest($"/api/TableModel/RemotePersonTable")
                .RespondingJsonContent(new TableStructureDto()
                {
                    DataType = typeof(Person).AssemblyQualifiedName,
                    IdType = typeof(int).AssemblyQualifiedName,
                    Id = "RemotePersonTable",
                    DataColumns = new ColumnDto[]
                    {
                        new ColumnDto()
                        {
                            Id = nameof(Person.FirstName),
                            DataGetterExpression = "p => p.FirstName",
                            DataType = typeof(string).AssemblyQualifiedName,
                        },
                        new ColumnDto()
                        {
                            Id = nameof(Person.LastName),
                            DataGetterExpression = "p => p.LastName",
                            DataType = typeof(string).AssemblyQualifiedName,
                        },
                        new ColumnDto()
                        {
                            Id = nameof(Person.Email),
                            DataGetterExpression = "p => p.Email",
                            DataType = typeof(string).AssemblyQualifiedName,
                        },
                        new ColumnDto()
                        {
                            Id = nameof(Person.BirthDate),
                            DataGetterExpression = "p => p.BirthDate",
                            DataType = typeof(DateTime).AssemblyQualifiedName,
                        },
                    },
                    IdColumn = new ColumnDto()
                    {
                        Id = nameof(Person.Id),
                        DataGetterExpression = "p => p.Id",
                        DataType = typeof(int).AssemblyQualifiedName,
                    }
                })
                .WithRequest($"/api/TableModel/RemotePersonTable/TextRemoteDecorator")
                .RespondingJsonContent(new TableDecoratorDto()
                {
                    DecoratorType = typeof(string).AssemblyQualifiedName,
                    Id = "TextRemoteDecorator",
                    DefaultDecoratorExpression = "v => v.ToString()",
                    DecoratorColumns = new []
                    {
                        new ColumnDecoratorDto()
                        {
                            Id = nameof(Person.LastName),
                            DecoratorExpression = "n => n.ToLower()",
                        },
                    }
                })
                .Build();

            var services = new ServiceCollection();

            services.AddTableStructure(
                builder =>
                {
                    builder
                        .UseRemoteTableStructure<Person, int>(
                            "RemotePersonTable",
                            config =>
                            {
                                config.HttpClient = httpClient;
                            })
                        .WithDecorator<string>(
                            "TextDecorator",
                             config =>
                             {
                                 config
                                     .AddDefault(v => v.ToString())
                                     .Add<string>(nameof(Person.LastName), n => n.ToUpper())
                                     .Add<DateTime>(nameof(Person.BirthDate), date => date.ToString("D", CultureInfo.InvariantCulture));
                             })
                        .WithRemoteDecorator<string>(
                            "TextRemoteDecorator",
                             config =>
                             {
                                 config.HttpClient = httpClient;
                             });

                    builder.UseRemoteTableData<Person>(
                        "RemotePersonTableData",
                        config =>
                        {
                            config.HttpClient = httpClient;
                        });
                });

            using var sp = services.BuildServiceProvider();

            var tableStructureRepository = sp.GetService<ITableStructureRepository>();

            Assert.NotNull(tableStructureRepository);

            var tableStructure = await tableStructureRepository.GetTableStructureAsync<Person, int>("RemotePersonTable");

            Assert.NotNull(tableStructure);

            var tableDecorator = await tableStructureRepository.GetTableDecoratorAsync<Person, string>("RemotePersonTable", "TextDecorator");

            Assert.NotNull(tableDecorator);

            var tableRemoteDecorator = await tableStructureRepository.GetTableDecoratorAsync<Person, string>("RemotePersonTable", "TextRemoteDecorator");

            Assert.NotNull(tableRemoteDecorator);

            var tableDataRepository = sp.GetService<ITableDataRepository>();

            Assert.NotNull(tableDataRepository);

            var tableData = await tableDataRepository.GetTableDataAsync<Person>("RemotePersonTableData");

            Assert.NotNull(tableData);
        }
    }
}
