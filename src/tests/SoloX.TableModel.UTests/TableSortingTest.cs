using FluentAssertions;
using SoloX.TableModel.Impl;
using SoloX.TableModel.UTests.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SoloX.TableModel.UTests
{
    public class TableSortingTest
    {
        [Fact]
        public async Task ItShouldProvideTheGivenDataWithSortingApplied()
        {
            var data = Person.GetSomePersons();

            var tableStructure = PersonEx.GetTableStructure();

            var tableSorting = new TableSorting<Person>();

            tableSorting.Register(tableStructure[nameof(Person.BirthDate)], SortingOrder.Ascending);

            var dataTable = new InMemoryTableData<Person>("dataId", data);

            var resultData = await dataTable.GetDataAsync(tableSorting);

            resultData.Should().NotBeNull();

            resultData.Count().Should().Be(7);

            resultData.Should().BeInAscendingOrder(p => p.BirthDate);
        }

    }
}
