// ----------------------------------------------------------------------
// <copyright file="AQueryableTableDataTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SoloX.CodeQuality.Test.Helpers.XUnit.Logger;
using SoloX.TableModel.Impl;
using SoloX.TableModel.UTests.Samples;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Globalization;

namespace SoloX.TableModel.UTests
{
    public class AQueryableTableDataTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public AQueryableTableDataTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task ItShouldReturnTheWholeDate()
        {
            await SetupDbContextAndRunTestAsync(async dbContext =>
            {
                var tableData = new FamilyMemberTableData("id", dbContext);

                var result = await tableData.GetDataAsync().ConfigureAwait(false);

                result.Should().NotBeNull();

                result.Select(p => p.FirstName).Should().BeEquivalentTo(dbContext.Persons.Select(p => p.FirstName));
            }).ConfigureAwait(false);
        }

        [Theory]
        [InlineData(0, 3)]
        [InlineData(0, 6)]
        [InlineData(3, 3)]
        [InlineData(2, 5)]
        [InlineData(4, 2)]
        public async Task ItShouldReturnThePageDate(int offset, int size)
        {
            await SetupDbContextAndRunTestAsync(async dbContext =>
            {
                var tableData = new FamilyMemberTableData("id", dbContext);

                var result = await tableData.GetDataPageAsync(offset, size).ConfigureAwait(false);

                result.Should().NotBeNull();

                result.Select(p => p.FirstName).Should().BeEquivalentTo(
                    dbContext.Persons.Skip(offset).Take(size).Select(p => p.FirstName));
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task ItShouldReturnTheFilteredDateUsingColumnFilter()
        {
            Expression<Func<string, bool>> filter = d => d == "Dolittle";

            Expression<Func<FamilyMemberDto, string>> data = d => d.FamilyName;

            await SetupDbContextAndRunTestAsync(async dbContext =>
            {
                var tableData = new FamilyMemberTableData("id", dbContext);

                var column = new Column<FamilyMemberDto, string>("col1", data);

                var tableFilter = new TableFilter<FamilyMemberDto>();

                tableFilter.Register(column, filter);

                var result = await tableData.GetDataAsync(tableFilter).ConfigureAwait(false);

                result.Should().NotBeNull();

                result.Select(p => p.FirstName).Should().BeEquivalentTo(dbContext.Persons.Where(p => p.Family.Name == "Dolittle").Select(p => p.FirstName));
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task ItShouldReturnTheFilteredDateUsingDataFilter()
        {
            Expression<Func<FamilyMemberDto, bool>> dataFilter = d => d.FamilyName == "Dolittle" && d.FirstName == "Lisa";

            await SetupDbContextAndRunTestAsync(async dbContext =>
            {
                var tableData = new FamilyMemberTableData("id", dbContext);

                var tableFilter = new TableFilter<FamilyMemberDto>();

                tableFilter.Register(dataFilter);

                var result = await tableData.GetDataAsync(tableFilter).ConfigureAwait(false);

                result.Should().NotBeNull();

                result.Select(p => p.FirstName).Should().BeEquivalentTo(
                    dbContext.Persons
                        .Where(p => p.Family.Name == "Dolittle" && p.FirstName == "Lisa")
                        .Select(p => p.FirstName));
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task ItShouldReturnTheFilteredDate2()
        {
            Expression<Func<string, bool>> filter = d => d.Contains("an");

            Expression<Func<FamilyMemberDto, string>> data = d => d.FirstName;

            await SetupDbContextAndRunTestAsync(async dbContext =>
            {
                var tableData = new FamilyMemberTableData("id", dbContext);

                var column = new Column<FamilyMemberDto, string>("col1", data);

                var tableFilter = new TableFilter<FamilyMemberDto>();

                tableFilter.Register(column, filter);

                var result = await tableData.GetDataAsync(tableFilter).ConfigureAwait(false);

                result.Should().NotBeNull();

                result.Select(p => p.FirstName).Should().BeEquivalentTo(dbContext.Persons.Where(p => p.FirstName.Contains("an")).Select(p => p.FirstName));
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task ItShouldReturnTheSortedDate()
        {
            Expression<Func<FamilyMemberDto, string>> data = d => d.FirstName;

            await SetupDbContextAndRunTestAsync(async dbContext =>
            {
                var tableData = new FamilyMemberTableData("id", dbContext);

                var column = new Column<FamilyMemberDto, string>("col1", data);

                var tableSorting = new TableSorting<FamilyMemberDto>();

                tableSorting.Register(column, SortingOrder.Descending);

                var result = await tableData.GetDataAsync(tableSorting).ConfigureAwait(false);

                result.Should().NotBeNull();

                result.Select(p => p.FirstName).Should().BeEquivalentTo(dbContext.Persons.OrderByDescending(p => p.FirstName).Select(p => p.FirstName));
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task ItShouldFilterOnGuid()
        {
            var guid = new Guid("f45132ed-e1cf-4ddf-b8f9-62e660d2b4cb");

            Expression<Func<Guid?, bool>> filter = d => d == null || d == new Guid("f45132ed-e1cf-4ddf-b8f9-62e660d2b4cb");

            Expression<Func<FamilyMemberDto, Guid?>> data = d => d.SomeGuid;

            await SetupDbContextAndRunTestAsync(async dbContext =>
            {
                var tableData = new FamilyMemberTableData("id", dbContext);

                var column = new Column<FamilyMemberDto, Guid?>("col1", data);

                var tableFilter = new TableFilter<FamilyMemberDto>();

                tableFilter.Register(column, filter);

                var result = await tableData.GetDataAsync(tableFilter).ConfigureAwait(false);

                result.Should().NotBeNull();

                result.Select(p => p.FirstName).Should().BeEquivalentTo(dbContext.Persons.Where(p => p.SomeGuid == null || p.SomeGuid == guid).Select(p => p.FirstName));
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task ItShouldSetupServiceCollectionWithQueryableTableData()
        {
            await SetupDbContextAndRunTestAsync(async dbContext =>
            {
                var services = new ServiceCollection();
                services.AddTransient(r => dbContext);

                services.AddTableModel(builder =>
                {
                    builder.UseQueryableTableData<FamilyMemberDto, FamilyMemberTableData>("MyTableId", options =>
                    {
                        options.Factory = (tableId, provider) =>
                            new FamilyMemberTableData(tableId, provider.GetRequiredService<FamilyDbContext>());
                    });
                });

                await using var sp = services.BuildServiceProvider();

                var tableDataRepository = sp.GetService<ITableDataRepository>();

                Assert.NotNull(tableDataRepository);

                var tableData = await tableDataRepository.GetTableDataAsync<FamilyMemberDto>("MyTableId").ConfigureAwait(false);

                Assert.NotNull(tableData);
                Assert.IsType<FamilyMemberTableData>(tableData);
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task ItShouldSetupServiceCollectionWithQueryableTableDataWithoutFactory()
        {
            await SetupDbContextAndRunTestAsync(async dbContext =>
            {
                var services = new ServiceCollection();
                services.AddTransient(r => dbContext);
                services.AddTransient<FamilyMemberTableData>();

                services.AddTableModel(builder =>
                {
                    builder.UseQueryableTableData<FamilyMemberDto, FamilyMemberTableData>("MyTableId");
                });

                await using var sp = services.BuildServiceProvider();

                var tableDataRepository = sp.GetService<ITableDataRepository>();

                Assert.NotNull(tableDataRepository);

                var tableData = await tableDataRepository.GetTableDataAsync<FamilyMemberDto>("MyTableId").ConfigureAwait(false);

                Assert.NotNull(tableData);
                Assert.Equal("MyTableId", tableData.Id);
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task ItShouldSetupServiceCollectionWithQueryableTableDataWithoutFactoryWithoutId()
        {
            await SetupDbContextAndRunTestAsync(async dbContext =>
            {
                var services = new ServiceCollection();
                services.AddTransient(r => dbContext);
                services.AddTransient<FamilyMemberTableData>();

                services.AddTableModel(builder =>
                {
                    builder.UseQueryableTableData<FamilyMemberDto, FamilyMemberTableData>();
                });

                await using var sp = services.BuildServiceProvider();

                var tableDataRepository = sp.GetService<ITableDataRepository>();

                Assert.NotNull(tableDataRepository);

                var tableData = await tableDataRepository.GetTableDataAsync<FamilyMemberDto>().ConfigureAwait(false);

                Assert.NotNull(tableData);
                Assert.Equal(typeof(FamilyMemberDto).FullName, tableData.Id);
                Assert.IsType<FamilyMemberTableData>(tableData);
            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task ItShouldApplyPostProcessing()
        {
            await SetupDbContextAndRunTestAsync(async dbContext =>
            {
                var tableData = new FamilyMemberTableData("id", dbContext, d =>
                {
                    d.FirstName = d.FirstName?.ToUpper(CultureInfo.InvariantCulture);
                    return d;
                });

                var result = await tableData.GetDataAsync().ConfigureAwait(false);

                result.Should().NotBeNull();

                result.Select(p => p.FirstName).Should().BeEquivalentTo(dbContext.Persons.Select(p => p.FirstName.ToUpper(CultureInfo.InvariantCulture)));

            }).ConfigureAwait(false);
        }

        [Fact]
        public async Task ItShouldApplyPostFiltering()
        {
            await SetupDbContextAndRunTestAsync(async dbContext =>
            {
                Expression<Func<FamilyMemberDto, bool>> postFiltering = d => d.FamilyName.StartsWith("Do");

                var tableData = new FamilyMemberTableData("id", dbContext, postFiltering);

                var result = await tableData.GetDataAsync().ConfigureAwait(false);

                result.Should().NotBeNull();

                result.Select(p => p.FirstName).Should().BeEquivalentTo(
                    dbContext.Persons.Where(p => p.Family.Name.StartsWith("Do")).Select(p => p.FirstName));

            }).ConfigureAwait(false);
        }

        private async Task SetupDbContextAndRunTestAsync(Func<FamilyDbContext, Task> testHandler)
        {
            await using var connection = new SqliteConnection("DataSource=:memory:");
            using var loggerFactory = new TestLoggerFactory(this.testOutputHelper);
            await connection.OpenAsync().ConfigureAwait(false);

            var options = new DbContextOptionsBuilder<FamilyDbContext>()
                .UseSqlite(connection) // Set the connection explicitly, so it won't be closed automatically by EF
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging()
                .Options;

            await using (var dbContext = new FamilyDbContext(options))
            {
                await dbContext.Database.EnsureCreatedAsync().ConfigureAwait(false);
            }

            await using (var dbContext = new FamilyDbContext(options))
            {
                await dbContext.Families.AddRangeAsync(Family.GetSomeFamilies()).ConfigureAwait(false);

                await dbContext.SaveChangesAsync().ConfigureAwait(false);

                await testHandler(dbContext).ConfigureAwait(false);
            }
        }

        internal class FamilyMemberTableData : AQueryableTableData<FamilyMemberDto>
        {
            private readonly FamilyDbContext familyDbContext;
            private readonly Func<FamilyMemberDto, FamilyMemberDto> postProcessing;
            private readonly Expression<Func<FamilyMemberDto, bool>> postFiltering;

            public FamilyMemberTableData(FamilyDbContext familyDbContext)
            {
                this.familyDbContext = familyDbContext;
            }

            public FamilyMemberTableData(string id, FamilyDbContext familyDbContext)
                : base(id)
            {
                this.familyDbContext = familyDbContext;
            }

            public FamilyMemberTableData(string id, FamilyDbContext familyDbContext, Expression<Func<FamilyMemberDto, bool>> postFiltering)
                : base(id)
            {
                this.familyDbContext = familyDbContext;
                this.postFiltering = postFiltering;
            }

            public FamilyMemberTableData(string id, FamilyDbContext familyDbContext, Func<FamilyMemberDto, FamilyMemberDto> postProcessing)
                : base(id)
            {
                this.familyDbContext = familyDbContext;
                this.postProcessing = postProcessing;
            }

            protected override IQueryable<FamilyMemberDto> ApplyPostFiltering(IQueryable<FamilyMemberDto> data)
            {
                return this.postFiltering != null ? data.Where(this.postFiltering) : data;
            }

            protected override Task<IEnumerable<FamilyMemberDto>> ApplyPostProcessingAsync(IQueryable<FamilyMemberDto> data)
            {
                return Task.FromResult<IEnumerable<FamilyMemberDto>>(this.postProcessing != null ? data.Select(this.postProcessing) : data);
            }

            protected override IQueryable<FamilyMemberDto> QueryData()
            {
                return this.familyDbContext.Persons
                        .Select(p => new FamilyMemberDto()
                        {
                            FamilyName = p.Family.Name,
                            FirstName = p.FirstName,
                            SomeGuid = p.SomeGuid,
                        });
            }
        }
    }
}
