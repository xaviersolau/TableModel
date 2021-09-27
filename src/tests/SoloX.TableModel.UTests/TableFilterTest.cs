using FluentAssertions;
using SoloX.TableModel.Impl;
using SoloX.TableModel.UTests.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;

namespace SoloX.TableModel.UTests
{
    public class TableFilterTest
    {
        [Fact]
        public async Task ItShouldProvideTheGivenDataWithFilterApplied()
        {
            var data = Person.GetSomePersons();

            var tableStructure = PersonEx.GetTableStructure();

            var tableFilter = new TableFilter<Person>();

            tableFilter.Register<DateTime>(tableStructure[nameof(Person.BirthDate)], date => date > new DateTime(3000, 1, 1));

            var dataTable = new InMemoryTableData<Person>("dataId", data);

            var resultData = await dataTable.GetDataAsync(tableFilter);

            resultData.Should().NotBeNull();

            resultData.Count().Should().Be(3);
        }
    }
}
