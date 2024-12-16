// ----------------------------------------------------------------------
// <copyright file="SetupTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SoloX.CodeQuality.Test.Helpers.XUnit;
using SoloX.TableModel.Server.Services;
using SoloX.TableModel.UTests.Samples;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.TableModel.Server.UTests
{
    public class SetupTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public SetupTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ItShouldSetupTableStructureServerServices()
        {
            var tableId = "LocalPersonTable";
            var services = new ServiceCollection();
            services
                .AddTestLogging(this.testOutputHelper)
                .AddTableModelServer(
                    builder =>
                    {
                        builder.UseTableStructure<Person, int>(
                                tableId,
                                config =>
                                {
                                    config
                                        .AddIdColumn(nameof(Person.Id), p => p.Id)
                                        .AddColumn(nameof(Person.FirstName), p => p.FirstName)
                                        .AddColumn(nameof(Person.LastName), p => p.LastName)
                                        .AddColumn(nameof(Person.Email), p => p.Email)
                                        .AddColumn(nameof(Person.BirthDate), p => p.BirthDate);
                                });
                    });

            using var sp = services.BuildServiceProvider();

            var tableStructureEndPointService = sp.GetService<ITableStructureEndPointService>();

            Assert.NotNull(tableStructureEndPointService);

            var registeredTableStructures = await tableStructureEndPointService.GetRegisteredTableStructuresAsync();

            Assert.NotNull(registeredTableStructures);
            registeredTableStructures.Single().Should().Be(tableId);
        }

        [Fact]
        public async Task ItShouldSetupTableDataServerServices()
        {
            var tableId = "TableId";
            var services = new ServiceCollection();

            services
                .AddTestLogging(this.testOutputHelper)
                .AddTableModelServer(
                    builder =>
                    {
                        builder.UseMemoryTableData<string>(
                            tableId,
                            config =>
                            {
                                config.AddData(["Hello"]);
                            });
                    });

            using var sp = services.BuildServiceProvider();

            var tableDataEndPointService = sp.GetService<ITableDataEndPointService>();

            Assert.NotNull(tableDataEndPointService);

            var registeredTableData = await tableDataEndPointService.GetRegisteredTableDataAsync();

            Assert.NotNull(registeredTableData);
            registeredTableData.Single().Id.Should().Be(tableId);
        }
    }
}
