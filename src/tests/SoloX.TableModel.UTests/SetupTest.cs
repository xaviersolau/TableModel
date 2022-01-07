// ----------------------------------------------------------------------
// <copyright file="SetupTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using SoloX.TableModel.UTests.Samples;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;
using SoloX.CodeQuality.Test.Helpers.Http;
using SoloX.TableModel.Dto;
using System.Net.Http;

namespace SoloX.TableModel.UTests
{
    public class SetupTest
    {
        private readonly Uri baseAddressUri = new Uri("http://host/api/TableModel");
        private readonly string apiTableModel = "/api/TableModel";

        [Fact]
        public async Task ItShouldSetupServicesFromLocal()
        {

            var services = new ServiceCollection();

            services.AddTableModel(
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
                                     .AddDefault(v => v.ToString(), c => $"{c.Id} [{c.DataType.Name}]")
                                     .Add<string>(nameof(Person.LastName), n => n.ToUpper(CultureInfo.InvariantCulture), () => "Last Name")
                                     .Add<DateTime>(nameof(Person.BirthDate), date => date.ToString("D", CultureInfo.InvariantCulture), () => "Birth Date");
                             })
                        .WithDecorator<string>(
                            "TextDecorator2",
                             config =>
                             {
                                 config
                                     .AddDefault(v => v.ToString(), c => c.Id)
                                     .Add<string>(nameof(Person.LastName), n => n.ToLower(CultureInfo.InvariantCulture), () => "Last Name");
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
        public async Task ItShouldSetupServicesFromLocalAndAutomaticIds()
        {

            var services = new ServiceCollection();

            services.AddTableModel(
                builder =>
                {
                    builder
                        .UseTableStructure<Person, int>(
                            config =>
                            {
                                config
                                    .AddIdColumn(p => p.Id)
                                    .AddColumn(p => p.FirstName)
                                    .AddColumn(p => p.LastName)
                                    .AddColumn(p => p.Email)
                                    .AddColumn(p => p.BirthDate);
                            })
                        .WithDecorator<string>(
                            "TextDecorator1",
                             config =>
                             {
                                 config
                                     .AddDefault(v => v.ToString(), c => $"{c.Id} [{c.DataType.Name}]")
                                     .Add(p => p.LastName, n => n.ToUpper(CultureInfo.InvariantCulture), () => "Last Name")
                                     .Add(p => p.BirthDate, date => date.ToString("D", CultureInfo.InvariantCulture), () => "Birth Date");
                             })
                        .WithDecorator<string>(
                            "TextDecorator2",
                             config =>
                             {
                                 config
                                     .AddDefault(v => v.ToString(), c => c.Id)
                                     .Add(p => p.LastName, n => n.ToLower(CultureInfo.InvariantCulture), () => "Last Name");
                             });

                    builder.UseMemoryTableData<Person>(
                        config =>
                        {
                            config.AddData(Person.GetSomePersons());
                        });
                });

            using var sp = services.BuildServiceProvider();

            var tableStructureRepository = sp.GetService<ITableStructureRepository>();

            Assert.NotNull(tableStructureRepository);

            var tableStructure = await tableStructureRepository.GetTableStructureAsync<Person, int>();

            Assert.NotNull(tableStructure);

            var tableDecorator1 = await tableStructureRepository.GetTableDecoratorAsync<Person, string>("TextDecorator1");

            Assert.NotNull(tableDecorator1);

            var tableDecorator2 = await tableStructureRepository.GetTableDecoratorAsync<Person, string>("TextDecorator2");

            Assert.NotNull(tableDecorator2);

            var tableDataRepository = sp.GetService<ITableDataRepository>();

            Assert.NotNull(tableDataRepository);

            var tableData = await tableDataRepository.GetTableDataAsync<Person>();

            Assert.NotNull(tableData);
        }

        [Fact]
        public async Task ItShouldSetupServicesFromRemote()
        {
            var httpClient = SetupHttpClientMockForTableStructure(this.baseAddressUri, this.apiTableModel, "RemotePersonTable");

            var services = new ServiceCollection();

            services.AddTableModel(
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
                                     .AddDefault(v => v.ToString(), c => $"{c.Id} [{c.DataType.Name}]")
                                     .Add<string>(nameof(Person.LastName), n => n.ToUpper(CultureInfo.InvariantCulture), () => "Last Name")
                                     .Add<DateTime>(nameof(Person.BirthDate), date => date.ToString("D", CultureInfo.InvariantCulture), () => "Birth Date");
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
                            config.HttpClient = SetupHttpClientMockForTableData(this.baseAddressUri);
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

        [Fact]
        public async Task ItShouldSetupServicesFromRemoteAndAutomaticIds()
        {
            var httpClient = SetupHttpClientMockForTableStructure(this.baseAddressUri, this.apiTableModel, typeof(Person).FullName);

            var services = new ServiceCollection();

            services.AddTableModel(
                builder =>
                {
                    builder
                        .UseRemoteTableStructure<Person, int>(
                            config =>
                            {
                                config.HttpClient = httpClient;
                            })
                        .WithDecorator<string>(
                            "TextDecorator",
                             config =>
                             {
                                 config
                                     .AddDefault(v => v.ToString(), c => $"{c.Id} [{c.DataType.Name}]")
                                     .Add(p => p.LastName, n => n.ToUpper(CultureInfo.InvariantCulture), () => "Last Name")
                                     .Add(p => p.BirthDate, date => date.ToString("D", CultureInfo.InvariantCulture), () => "Birth Date");
                             })
                        .WithRemoteDecorator<string>(
                            "TextRemoteDecorator",
                             config =>
                             {
                                 config.HttpClient = httpClient;
                             });

                    builder.UseRemoteTableData<Person>(
                        config =>
                        {
                            config.HttpClient = SetupHttpClientMockForTableData(this.baseAddressUri);
                        });
                });

            using var sp = services.BuildServiceProvider();

            var tableStructureRepository = sp.GetService<ITableStructureRepository>();

            Assert.NotNull(tableStructureRepository);

            var tableStructure = await tableStructureRepository.GetTableStructureAsync<Person, int>();

            Assert.NotNull(tableStructure);

            var tableDecorator = await tableStructureRepository.GetTableDecoratorAsync<Person, string>("TextDecorator");

            Assert.NotNull(tableDecorator);

            var tableRemoteDecorator = await tableStructureRepository.GetTableDecoratorAsync<Person, string>("TextRemoteDecorator");

            Assert.NotNull(tableRemoteDecorator);

            var tableDataRepository = sp.GetService<ITableDataRepository>();

            Assert.NotNull(tableDataRepository);

            var tableData = await tableDataRepository.GetTableDataAsync<Person>();

            Assert.NotNull(tableData);
        }

        private static HttpClient SetupHttpClientMockForTableData(Uri baseAddressUri)
        {
            return new HttpClientMockBuilder().WithBaseAddress(baseAddressUri).Build();
        }

        private static HttpClient SetupHttpClientMockForTableStructure(Uri baseAddressUri, string apiTableModel, string remoteTableStructureId)
        {
            var httpClientBuilder = new HttpClientMockBuilder();

            return httpClientBuilder
                .WithBaseAddress(baseAddressUri)
                .WithRequest($"{apiTableModel}/{remoteTableStructureId}/structure")
                .RespondingJsonContent(new TableStructureDto()
                {
                    DataType = typeof(Person).AssemblyQualifiedName,
                    IdType = typeof(int).AssemblyQualifiedName,
                    Id = remoteTableStructureId,
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
                .WithRequest($"{apiTableModel}/{remoteTableStructureId}/TextRemoteDecorator")
                .RespondingJsonContent(new TableDecoratorDto()
                {
                    DecoratorType = typeof(string).AssemblyQualifiedName,
                    Id = "TextRemoteDecorator",
                    DefaultDecoratorExpression = "v => v.ToString()",
                    DefaultHeaderDecoratorExpression = "c => c.Id",
                    DecoratorColumns = new ColumnDecoratorDto[]
                    {
                        new ColumnDecoratorDto()
                        {
                            Id = nameof(Person.LastName),
                            DecoratorExpression = "n => n.ToLower()",
                            HeaderDecoratorExpression = "() => \"LastName\""
                        },
                    }
                })
                .Build();
        }
    }
}
