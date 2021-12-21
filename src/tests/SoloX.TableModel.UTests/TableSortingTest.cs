// ----------------------------------------------------------------------
// <copyright file="TableSortingTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using SoloX.TableModel.Impl;
using SoloX.TableModel.UTests.Samples;
using System.Linq;
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
