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
        public async Task ItShouldReturnTheFilteredDate()
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
        public async Task ItShouldReturnTheFilteredDate2()
        {
            Expression<Func<string, bool>> filter = d => d.Contains("a");

            Expression<Func<FamilyMemberDto, string>> data = d => d.FirstName;

            await SetupDbContextAndRunTestAsync(async dbContext =>
            {
                var tableData = new FamilyMemberTableData("id", dbContext);

                var column = new Column<FamilyMemberDto, string>("col1", data);

                var tableFilter = new TableFilter<FamilyMemberDto>();

                tableFilter.Register(column, filter);

                var result = await tableData.GetDataAsync(tableFilter).ConfigureAwait(false);

                result.Should().NotBeNull();

                result.Select(p => p.FirstName).Should().BeEquivalentTo(dbContext.Persons.Where(p => p.FirstName.Contains("a")).Select(p => p.FirstName));
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
        public async Task ItShouldSetupWithoutFactoryServiceCollectionWithQueryableTableData()
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
        public async Task ItShouldSetupWithoutFactoryWithoutIdServiceCollectionWithQueryableTableData()
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

            public FamilyMemberTableData(FamilyDbContext familyDbContext)
            {
                this.familyDbContext = familyDbContext;
            }

            public FamilyMemberTableData(string id, FamilyDbContext familyDbContext)
                : base(id)
            {
                this.familyDbContext = familyDbContext;
            }

            protected override IQueryable<FamilyMemberDto> QueryData()
            {
                return this.familyDbContext.Persons
                        .Select(p => new FamilyMemberDto()
                        {
                            FamilyName = p.Family.Name,
                            FirstName = p.FirstName,
                        });
            }
        }
    }
}
