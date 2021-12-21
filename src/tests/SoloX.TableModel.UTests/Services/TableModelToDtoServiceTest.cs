// ----------------------------------------------------------------------
// <copyright file="TableModelToDtoServiceTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using SoloX.TableModel.Impl;
using SoloX.TableModel.Services.Impl;
using SoloX.TableModel.UTests.Samples;
using System;
using System.Linq;
using Moq;
using Xunit;
using System.Linq.Expressions;
using System.Globalization;

namespace SoloX.TableModel.UTests.Services
{
    public class TableModelToDtoServiceTest
    {
        [Fact]
        public void ItShouldMapTableColumnToDto()
        {
            var column = new Column<Person, string>(nameof(Person.FirstName), p => p.FirstName);

            var service = new TableModelToDtoService();

            var dto = service.Map(column);

            dto.Should().NotBeNull();

            dto.Id.Should().Be(nameof(Person.FirstName));

            dto.DataGetterExpression.Should().BeEquivalentTo(column.DataGetterExpression.ToString());

        }

        [Fact]
        public void ItShouldMapTableStructureToDto()
        {
            var tableStructure = PersonEx.GetTableStructure();

            var service = new TableModelToDtoService();

            var dto = service.Map(tableStructure);

            dto.Should().NotBeNull();

            dto.DataColumns.Should().NotBeNull();
            dto.DataColumns.Count().Should().Be(tableStructure.DataColumns.Count());

            dto.IdColumn.Should().NotBeNull();
        }

        [Fact]
        public void ItShouldMapTableDecoratorColumnToDto()
        {
            Expression<Func<Person, string>> dataExpression = p => p.FirstName;

            var columnMock = new Mock<IColumn<Person, string>>();
            columnMock.SetupGet(x => x.Id).Returns(nameof(Person.FirstName));
            columnMock.SetupGet(x => x.DataGetterExpression).Returns(dataExpression);

            var columnDecorator = new ColumnDecorator<Person, string, string>(columnMock.Object, p => p.ToUpper(CultureInfo.InvariantCulture));

            var service = new TableModelToDtoService();

            var dto = service.Map(columnDecorator);

            dto.Should().NotBeNull();

            dto.Id.Should().Be(nameof(Person.FirstName));

            dto.DecoratorExpression.Should().BeEquivalentTo(columnDecorator.RelativeDecoratorExpression.ToString());
        }

        [Fact]
        public void ItShouldMapTableDecoratorToDto()
        {
            var tableStructure = PersonEx.GetTableStructure();
            var tableDecorator = PersonEx.GetTableDecorator(tableStructure);

            var service = new TableModelToDtoService();

            var dto = service.Map(tableDecorator);

            dto.Should().NotBeNull();

            dto.Id.Should().Be(tableDecorator.Id);
            dto.DecoratorColumns.Count().Should().Be(tableDecorator.TableColumnDecorators.Count());

            dto.DecoratorType.Should().Be(tableDecorator.DecoratorType.AssemblyQualifiedName);
        }
    }
}
