// ----------------------------------------------------------------------
// <copyright file="DtoToTableModelServiceTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using SoloX.TableModel.Dto;
using SoloX.TableModel.Services.Impl;
using SoloX.TableModel.UTests.Samples;
using System;
using System.Linq.Expressions;
using Moq;
using Xunit;

namespace SoloX.TableModel.UTests.Services
{
#pragma warning disable CA1304 // Spécifier CultureInfo
    public class DtoToTableModelServiceTest
    {
        [Fact]
        public void ItShouldMapDtoToTableColumn()
        {
            Expression<Func<Person, string>> expression = p => p.FirstName;

            var dto = new ColumnDto()
            {
                DataType = typeof(string).AssemblyQualifiedName,
                DataGetterExpression = expression.ToString(),
            };

            var service = new DtoToTableModelService();

            var column = service.Map<Person>(dto);

            column.Should().NotBeNull();

            var persone = new Person()
            {
                FirstName = "ANAKIN",
                LastName = "SKYWALKER",
                BirthDate = new DateTime(3054, 5, 24),
            };

            column.GetObject(persone).Should().Be(persone.FirstName);
        }

        [Fact]
        public void ItShouldMapDtoToTableStructure()
        {
            Expression<Func<Person, int>> idExpression = p => p.Id;

            Expression<Func<Person, string>> expression = p => p.FirstName;

            var dto = new TableStructureDto()
            {
                DataType = typeof(Person).AssemblyQualifiedName,
                IdType = typeof(int).AssemblyQualifiedName,
                IdColumn = new ColumnDto()
                {
                    Id = nameof(Person.Id),
                    DataType = typeof(int).FullName,
                    DataGetterExpression = idExpression.ToString(),
                },
                DataColumns = new ColumnDto[]
                {
                    new ColumnDto()
                    {
                        Id = nameof(Person.FirstName),
                        DataType = typeof(string).FullName,
                        DataGetterExpression = expression.ToString(),
                    },
                }
            };

            var service = new DtoToTableModelService();

            var structure = service.Map<Person, int>(dto);

            structure.Should().NotBeNull();
        }

        [Fact]
        public void ItShouldMapDtoToTableColumnDecorator()
        {
            Expression<Func<string, string>> relativeExpression = p => p.ToUpperInvariant();
            Expression<Func<Person, string>> columnExpression = p => p.FirstName;

            Expression<Func<string>> headerExpression = () => "FirstName";

            var columnMock = new Mock<IColumn<Person, string>>();
            columnMock
                .SetupGet(x => x.DataGetterExpression)
                .Returns(columnExpression);

            var dto = new ColumnDecoratorDto()
            {
                Id = nameof(Person.FirstName),
                DecoratorExpression = relativeExpression.ToString(),
                HeaderDecoratorExpression = headerExpression.ToString(),
            };

            var service = new DtoToTableModelService();

            var columnDecorator = service.Map<Person, string, string>(dto, columnMock.Object);

            columnDecorator.Should().NotBeNull();

            var persone = new Person()
            {
                FirstName = "Anakin",
                LastName = "SKYWALKER",
                BirthDate = new DateTime(3054, 5, 24),
            };

            columnDecorator.Decorate(persone).Should().Be(persone.FirstName.ToUpperInvariant());
            columnDecorator.DecorateHeader().Should().Be("FirstName");
        }

        [Fact]
        public void ItShouldMapDtoToTableDecorator()
        {
#pragma warning disable CA1308 // Normalize strings to uppercase
            Expression<Func<object, string>> relativeDefaultExpression = p => p.ToString().ToLowerInvariant();
#pragma warning restore CA1308 // Normalize strings to uppercase
            Expression<Func<string, string>> relativeExpression = p => p.ToUpperInvariant();

            Expression<Func<IColumn<Person>, string>> defaultHeaderExpression = c => c.Id;
            Expression<Func<string>> headerExpression = () => "First Name";

            var tableStructure = PersonEx.GetTableStructure();
            var column = tableStructure[nameof(Person.FirstName)];

            var dto = new TableDecoratorDto()
            {
                Id = "SomeDecoratorId",
                DecoratorType = typeof(string).AssemblyQualifiedName,
                DefaultDecoratorExpression = relativeDefaultExpression.ToString(),
                DefaultHeaderDecoratorExpression = defaultHeaderExpression.ToString(),
                DecoratorColumns = new ColumnDecoratorDto[]
                {
                    new ColumnDecoratorDto()
                    {
                        Id = nameof(Person.FirstName),
                        DecoratorExpression = relativeExpression.ToString(),
                        HeaderDecoratorExpression = headerExpression.ToString(),
                    },
                }
            };

            var service = new DtoToTableModelService();

            var decorator = service.Map<Person, string>(dto, tableStructure);

            decorator.Should().NotBeNull();
            decorator.Id.Should().Be(dto.Id);

            decorator.TableStructure.Should().Be(tableStructure);
            decorator.TableColumnDecorators.Should().ContainSingle(x => x.Column == column);

            var persone = new Person()
            {
                FirstName = "Anakin",
                LastName = "SKYWALKER",
                BirthDate = new DateTime(3054, 5, 24),
            };

            decorator.Decorate(column, persone)
                .Should().Be(persone.FirstName.ToUpperInvariant());

#pragma warning disable CA1308 // Normalize strings to uppercase
            decorator.Decorate(tableStructure[nameof(Person.LastName)], persone)
                .Should().Be(persone.LastName.ToLowerInvariant());
#pragma warning restore CA1308 // Normalize strings to uppercase

            decorator.DecorateHeader(tableStructure[nameof(Person.LastName)])
                .Should().Be(nameof(Person.LastName));

            decorator.DecorateHeader(tableStructure[nameof(Person.FirstName)])
                .Should().Be("First Name");
        }
    }
#pragma warning restore CA1304 // Spécifier CultureInfo
}
