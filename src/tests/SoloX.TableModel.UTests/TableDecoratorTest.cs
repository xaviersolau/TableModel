using FluentAssertions;
using SoloX.TableModel.Impl;
using SoloX.TableModel.UTests.Samples;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SoloX.TableModel.UTests
{
    public class TableDecoratorTest
    {
        [Fact]
        public void ItShouldFormatTheColumns()
        {
            var person = Person.GetJohnDoe();

            var tableStructure = PersonEx.GetTableStructure();
            var decorator = PersonEx.GetTableDecorator(tableStructure);

            var id = decorator.Decorate(tableStructure[nameof(Person.Id)], person);
            var first = decorator.Decorate(tableStructure[nameof(Person.FirstName)], person);
            var last = decorator.Decorate(tableStructure[nameof(Person.LastName)], person);
            var birth = decorator.Decorate(tableStructure[nameof(Person.BirthDate)], person);

            id.Should().BeEquivalentTo(person.Id.ToString());
            first.Should().BeEquivalentTo(person.FirstName);
            last.Should().BeEquivalentTo(person.LastName.ToUpper());
            birth.Should().BeEquivalentTo(person.BirthDate.ToString("D", CultureInfo.InvariantCulture));
        }
    }
}
