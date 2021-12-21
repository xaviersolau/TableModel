// ----------------------------------------------------------------------
// <copyright file="TableStructureTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.TableModel.UTests.Samples;
using Xunit;
using FluentAssertions;

namespace SoloX.TableModel.UTests
{
    public class TableStructureTest
    {
        [Fact]
        public void ItShouldReturnDataForEachColumns()
        {
            var person = Person.GetJohnDoe();

            var tableStructure = PersonEx.GetTableStructure();

            var id = tableStructure[nameof(Person.Id)].GetObject(person);
            var first = tableStructure[nameof(Person.FirstName)].GetObject(person);
            var last = tableStructure[nameof(Person.LastName)].GetObject(person);
            var birth = tableStructure[nameof(Person.BirthDate)].GetObject(person);

            id.Should().BeEquivalentTo(person.Id);
            first.Should().BeEquivalentTo(person.FirstName);
            last.Should().BeEquivalentTo(person.LastName);
            birth.Should().BeEquivalentTo(person.BirthDate);

        }
    }
}
