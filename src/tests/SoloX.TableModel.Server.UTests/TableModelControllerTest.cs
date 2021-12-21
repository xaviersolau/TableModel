// ----------------------------------------------------------------------
// <copyright file="TableModelControllerTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoloX.TableModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SoloX.TableModel.Impl;
using SoloX.TableModel.UTests.Samples;
using Xunit;
using SoloX.TableModel.Services;
using System.Linq;

namespace SoloX.TableModel.Server.UTests
{
    public class TableModelControllerTest
    {
        [Fact]
        public async Task ItShouldGetFilteredDataThoughTheController()
        {
            Expression<Func<string, bool>> dataFilterExp = d => d.Contains("J");
            Expression<Func<Person, string>> dataGetterExp = d => d.FirstName;


            var tableId = "id";
            var inMemoryTableData = new InMemoryTableData<Person>(tableId, Person.GetSomePersons());

            var columnDto = new ColumnDto()
            {
                Id = "columnId",
                DataType = typeof(string).AssemblyQualifiedName,
                DataGetterExpression = dataGetterExp.ToString(),
            };

            var request = new DataRequestDto()
            {
                Filters = new FilterDto[]
                {
                    new FilterDto()
                    {
                        Column = columnDto,
                        FilterExpression = dataFilterExp.ToString(),
                    },
                }
            };

            var repositoryMock = new Mock<ITableDataRepository>();
            repositoryMock.Setup(r => r.GetTableDataAsync(tableId)).ReturnsAsync(inMemoryTableData);

            var dtoToTableModelServiceMock = new Mock<IDtoToTableModelService>();

            dtoToTableModelServiceMock
                .Setup(s => s.Map<Person>(columnDto))
                .Returns(new Column<Person, string>(columnDto.Id, dataGetterExp));

            var controller = new TableModelController(repositoryMock.Object, dtoToTableModelServiceMock.Object);

            var data = await controller.PostDataRequestAsync(tableId, request);

            data.Should().NotBeNull();

            var result = data.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeAssignableTo<IEnumerable<Person>>().Which;

            result.Should().BeEquivalentTo(Person.GetSomePersons().Where(p => p.FirstName.Contains("J", StringComparison.InvariantCulture)));
        }
    }
}
