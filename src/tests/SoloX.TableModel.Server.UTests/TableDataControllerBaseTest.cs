// ----------------------------------------------------------------------
// <copyright file="TableDataControllerBaseTest.cs" company="Xavier Solau">
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
    public class TableDataControllerBaseTest
    {
        [Fact]
        public async Task ItShouldGetIndexFromController()
        {
            var tableDataList = new TableDataDto[]
            {
                new TableDataDto()
                {
                    Id = "abc123",
                }
            };

            var endPointServiceMock = new Mock<ITableDataEndPointService>();

            endPointServiceMock.Setup(x => x.GetRegisteredTableDataAsync()).ReturnsAsync(tableDataList);

            var controller = new TableDataController(endPointServiceMock.Object);

            var indexResult = await controller.IndexAsync();

            Assert.NotNull(indexResult);

            var okResult = Assert.IsType<OkObjectResult>(indexResult);

            okResult.Value.Should().BeEquivalentTo(tableDataList);
        }

        private class TableDataController : TableDataControllerBase
        {
            public TableDataController(ITableDataEndPointService tableDataEndPointService)
                : base(tableDataEndPointService)
            {
            }
        }
    }
}
