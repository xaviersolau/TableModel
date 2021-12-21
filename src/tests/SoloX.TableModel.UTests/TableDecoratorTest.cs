// ----------------------------------------------------------------------
// <copyright file="TableDecoratorTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using SoloX.TableModel.UTests.Samples;
using System.Globalization;
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

            id.Should().BeEquivalentTo(person.Id.ToString(CultureInfo.InvariantCulture));
            first.Should().BeEquivalentTo(person.FirstName);
            last.Should().BeEquivalentTo(person.LastName.ToUpper(CultureInfo.InvariantCulture));
            birth.Should().BeEquivalentTo(person.BirthDate.ToString("D", CultureInfo.InvariantCulture));
        }
    }
}
