// ----------------------------------------------------------------------
// <copyright file="TableStructureEndPointServiceTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Moq;
using SoloX.TableModel.Dto;
using SoloX.TableModel.Server.Services.Impl;
using SoloX.TableModel.Services;
using SoloX.TableModel.UTests.Samples;
using System.Threading.Tasks;
using Xunit;

namespace SoloX.TableModel.Server.UTests.Services
{
    public class TableStructureEndPointServiceTest
    {
        [Fact]
        public async Task ItShouldGetTableStructureThoughTheEndPoint()
        {
            var tableId = "id";

            var tableStructure = PersonEx.GetTableStructure();

            var repositoryMock = new Mock<ITableStructureRepository>();
            repositoryMock.Setup(r => r.GetTableStructureAsync(tableId)).ReturnsAsync(tableStructure);

            var tableToDtoModelServiceMock = new Mock<ITableModelToDtoService>();

            tableToDtoModelServiceMock
                .Setup(s => s.Map(tableStructure))
                .Returns(new TableStructureDto()
                {
                    Id = tableId,
                });

            var endPointService = new TableStructureEndPointService(repositoryMock.Object, tableToDtoModelServiceMock.Object);

            var tableDto = await endPointService.GetTableStructureAsync(tableId);

            tableDto.Should().NotBeNull();

            tableDto.Id.Should().Be(tableId);
        }
    }
}
