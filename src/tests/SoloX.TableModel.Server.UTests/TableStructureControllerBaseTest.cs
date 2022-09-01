// ----------------------------------------------------------------------
// <copyright file="TableStructureControllerBaseTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SoloX.TableModel.Dto;
using SoloX.TableModel.Server.Services;
using System.Threading.Tasks;
using Xunit;

namespace SoloX.TableModel.Server.UTests
{
    public class TableStructureControllerBaseTest
    {
        [Fact]
        public async Task ItShouldGetIndexFromController()
        {
            var strIds = new string[] { "a", "b", "c" };

            var endPointServiceMock = new Mock<ITableStructureEndPointService>();

            endPointServiceMock.Setup(x => x.GetRegisteredTableStructuresAsync()).ReturnsAsync(strIds);

            var controller = new TableStructureController(endPointServiceMock.Object);

            var indexResult = await controller.IndexAsync();

            Assert.NotNull(indexResult);

            var okResult = Assert.IsType<OkObjectResult>(indexResult);

            okResult.Value.Should().BeEquivalentTo(strIds);
        }

        [Fact]
        public async Task ItShouldGetTableStructureFromController()
        {
            var strId = "abc123";

            var strDto = new TableStructureDto()
            {
                Id = strId,
            };

            var endPointServiceMock = new Mock<ITableStructureEndPointService>();

            endPointServiceMock.Setup(x => x.GetTableStructureAsync(strId)).ReturnsAsync(strDto);

            var controller = new TableStructureController(endPointServiceMock.Object);

            var tableStructureResult = await controller.GetTableStructureAsync(strId);

            Assert.NotNull(tableStructureResult);

            var okResult = Assert.IsType<OkObjectResult>(tableStructureResult);

            okResult.Value.Should().BeEquivalentTo(strDto);
        }

        private class TableStructureController : TableStructureControllerBase
        {
            public TableStructureController(ITableStructureEndPointService tableStructureEndPointService)
                : base(tableStructureEndPointService)
            {
            }
        }
    }
}
