using SoloX.TableModel.Impl;
using SoloX.TableModel.UTests.Samples;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
