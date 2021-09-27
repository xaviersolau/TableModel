using FluentAssertions;
using SoloX.TableModel.Impl;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SoloX.TableModel.UTests.Samples;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using SoloX.CodeQuality.Test.Helpers.XUnit.Logger;
using Xunit.Abstractions;

namespace SoloX.TableModel.UTests
{
    public class InMemoryTableDataTest
    {
        [Fact]
        public async Task ItShouldProvideTheGivenData()
        {
            var data = SetupInMemoryTableData(out var dataTable);

            var resultData = await dataTable.GetDataAsync();

            resultData.Should().NotBeNull();
            resultData.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task ItShouldReturnDataCount()
        {
            var data = SetupInMemoryTableData(out var dataTable);

            var resultData = await dataTable.GetDataCountAsync();

            resultData.Should().Be(5);
        }

        [Fact]
        public async Task ItShouldReturnFilteredDataCount()
        {
            var data = SetupInMemoryTableData(out var dataTable);

            var filter = new TableFilter<string>();
            filter.Register("id", s => s, s => s.Contains("data3"));

            var resultData = await dataTable.GetDataCountAsync(filter);

            resultData.Should().Be(1);
        }


        private static string[] SetupInMemoryTableData(out InMemoryTableData<string> dataTable)
        {
            var data = new[]
            {
                "data1",
                "data2",
                "data3",
                "data4",
                "data5",
            };

            dataTable = new InMemoryTableData<string>("dataId", data);
            return data;
        }

        [Theory]
        [InlineData(0, 3)]
        [InlineData(0, 6)]
        [InlineData(3, 3)]
        [InlineData(2, 5)]
        [InlineData(4, 2)]
        public async Task ItShouldProvideDataByPage(int offset, int size)
        {
            var data = new[]
            {
                0,
                1,
                2,
                3,
                4,
                5,
            };

            var dataTable = new InMemoryTableData<int>("dataId", data);

            var resultData = await dataTable.GetDataPageAsync(offset, size);

            resultData.Should().NotBeNull();
            resultData.Should().BeEquivalentTo(data.Skip(offset).Take(size));
        }
    }
}
