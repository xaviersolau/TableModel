// ----------------------------------------------------------------------
// <copyright file="TableFilterTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using SoloX.TableModel.Impl;
using SoloX.TableModel.UTests.Samples;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SoloX.TableModel.UTests
{
    public class TableFilterTest
    {
        [Fact]
        public async Task ItShouldProvideTheGivenDataWithFilterAppliedUsingColumnFilter()
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

        [Fact]
        public async Task ItShouldProvideTheGivenDataWithFilterAppliedUsingDataFilter()
        {
            var data = Person.GetSomePersons();

            var tableStructure = PersonEx.GetTableStructure();

            var tableFilter = new TableFilter<Person>();

            tableFilter.Register(data => data.BirthDate > new DateTime(3000, 1, 1));

            var dataTable = new InMemoryTableData<Person>("dataId", data);

            var resultData = await dataTable.GetDataAsync(tableFilter);

            resultData.Should().NotBeNull();

            resultData.Count().Should().Be(3);
        }
    }
}
