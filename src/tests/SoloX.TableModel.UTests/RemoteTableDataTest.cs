// ----------------------------------------------------------------------
// <copyright file="RemoteTableDataTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Moq;
using SoloX.CodeQuality.Test.Helpers.Http;
using SoloX.TableModel.Dto;
using SoloX.TableModel.Impl;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SoloX.TableModel.UTests
{
    public class RemoteTableDataTest
    {
        [Fact]
        public async Task ItShouldProvideAllRemoteData()
        {
            var data = new[]
            {
                "data1",
                "data2",
                "data3",
                "data4",
                "data5",
            };

            var baseAddress = "http://host/api/TableModel";
            var httpClientBuilder = new HttpClientMockBuilder();

            var httpClient = httpClientBuilder
                .WithBaseAddress(new Uri(baseAddress))
                .WithJsonContentRequest<DataRequestDto>($"/api/TableModel/RemotePersonData/Data", HttpMethod.Post)
                .RespondingJsonContent(
                    request =>
                    {
                        request.Should().NotBeNull();

                        request.Should().BeEquivalentTo(new DataRequestDto());

                        return data;
                    })
                .Build();


            var dataTable = new RemoteTableData<string>("RemotePersonData", httpClient, "Data", "Count");

            var resultData = await dataTable.GetDataAsync();

            resultData.Should().NotBeNull();
            resultData.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task ItShouldProvideRemoteDataPage()
        {
            var data = new[]
            {
                "data1",
                "data2",
                "data3",
                "data4",
                "data5",
            };

            var baseAddress = "http://host/api/TableModel";
            var httpClientBuilder = new HttpClientMockBuilder();

            var httpClient = httpClientBuilder
                .WithBaseAddress(new Uri(baseAddress))
                .WithJsonContentRequest<DataRequestDto>($"/api/TableModel/RemotePersonData/Data", HttpMethod.Post)
                .RespondingJsonContent(
                    request =>
                    {
                        request.Should().NotBeNull();

                        request.Should().BeEquivalentTo(new DataRequestDto()
                        {
                            PageSize = 2,
                            Offset = 1,
                        });

                        return data.Skip(1).Take(2);
                    })
                .Build();


            var dataTable = new RemoteTableData<string>("RemotePersonData", httpClient, "Data", "Count");

            var resultData = await dataTable.GetDataPageAsync(1, 2);

            resultData.Should().NotBeNull();
            resultData.Should().BeEquivalentTo(
            [
                "data2",
                "data3",
            ]);
        }

        [Fact]
        public async Task ItShouldProvideRemoteDataCount()
        {
            var baseAddress = "http://host/api/TableModel";
            var httpClientBuilder = new HttpClientMockBuilder();

            var httpClient = httpClientBuilder
                .WithBaseAddress(new Uri(baseAddress))
                .WithJsonContentRequest<DataCountRequestDto>($"/api/TableModel/RemotePersonData/Count", HttpMethod.Post)
                .RespondingJsonContent(
                    request =>
                    {
                        request.Should().NotBeNull();

                        request.Should().BeEquivalentTo(new DataCountRequestDto()
                        {
                        });

                        return 6;
                    })
                .Build();


            var dataTable = new RemoteTableData<string>("RemotePersonData", httpClient, "Data", "Count");

            var resultData = await dataTable.GetDataCountAsync();

            resultData.Should().Be(6);
        }

        [Fact]
        public async Task ItShouldProvideRemoteFilteredDataCountUsingColumnFilter()
        {
            var columnId = "columnId";
            Expression<Func<string, bool>> dataFilterExp = d => d.Contains("data3");
            Expression<Func<string, string>> dataGetterExp = d => d;

            var columnMock = SetupColumnMock(columnId, dataGetterExp);

            var filterMock = SetupColumnFilterMock(dataFilterExp, columnMock);

            var filterDto = SetupFilterDto(dataFilterExp, SetupColumnDto(columnId, dataGetterExp));

            await ProvideRemoteFilteredDataCount(filterMock, filterDto).ConfigureAwait(false);
        }

        [Fact]
        public async Task ItShouldProvideRemoteFilteredDataCountUsingDataFilter()
        {
            Expression<Func<string, bool>> dataFilterExp = d => d.Contains("data3");

            var filterMock = SetupDataFilterMock(dataFilterExp);

            var filterDto = SetupFilterDto(dataFilterExp);

            await ProvideRemoteFilteredDataCount(filterMock, filterDto).ConfigureAwait(false);
        }

        private static async Task ProvideRemoteFilteredDataCount(Mock<ITableFilter<string>> filterMock, FilterDto filterDto)
        {
            var baseAddress = "http://host/api/TableModel";
            var httpClientBuilder = new HttpClientMockBuilder();

            var httpClient = httpClientBuilder
                .WithBaseAddress(new Uri(baseAddress))
                .WithJsonContentRequest<DataCountRequestDto>($"/api/TableModel/RemotePersonData/Count", HttpMethod.Post)
                .RespondingJsonContent(
                    request =>
                    {
                        request.Should().NotBeNull();

                        request.Should().BeEquivalentTo(new DataCountRequestDto()
                        {
                            Filters = new[]
                            {
                                filterDto
                            },
                        });

                        return 6;
                    })
                .Build();


            var dataTable = new RemoteTableData<string>("RemotePersonData", httpClient, "Data", "Count");

            var resultData = await dataTable.GetDataCountAsync(filterMock.Object);

            resultData.Should().Be(6);
        }

        [Fact]
        public async Task ItShouldProvideRemoteDataFilteredUsingColumnFilter()
        {
            var columnId = "columnId";

            Expression<Func<string, bool>> dataFilterExp = d => d.Contains("data3");
            Expression<Func<string, string>> dataGetterExp = d => d;

            var columnMock = SetupColumnMock(columnId, dataGetterExp);

            var filterMock = SetupColumnFilterMock(dataFilterExp, columnMock);

            var filterDto = SetupFilterDto(dataFilterExp, SetupColumnDto(columnId, dataGetterExp));

            await ProvideRemoteDataFiltered(filterMock, filterDto).ConfigureAwait(false);
        }

        [Fact]
        public async Task ItShouldProvideRemoteDataFilteredUsingDataFilter()
        {
            Expression<Func<string, bool>> dataFilterExp = d => d.Contains("data3");

            var filterMock = SetupDataFilterMock(dataFilterExp);

            var filterDto = SetupFilterDto(dataFilterExp);

            await ProvideRemoteDataFiltered(filterMock, filterDto).ConfigureAwait(false);
        }

        private static async Task ProvideRemoteDataFiltered(Mock<ITableFilter<string>> filterMock, FilterDto filterDto)
        {
            var data = new[]
                        {
                "data1",
                "data2",
                "data3",
                "data4",
                "data5",
            };

            var baseAddress = "http://host/api/TableModel";
            var httpClientBuilder = new HttpClientMockBuilder();

            var httpClient = httpClientBuilder
                .WithBaseAddress(new Uri(baseAddress))
                .WithJsonContentRequest<DataRequestDto>($"/api/TableModel/RemotePersonData/Data", HttpMethod.Post)
                .RespondingJsonContent(
                    request =>
                    {
                        request.Should().NotBeNull();

                        request.Should().BeEquivalentTo(new DataRequestDto()
                        {
                            PageSize = 2,
                            Offset = 1,
                            Filters = new[]
                            {
                                filterDto,
                            },
                        });

                        return data.Where(d => d.Contains("data3", StringComparison.Ordinal));
                    })
                .Build();


            var dataTable = new RemoteTableData<string>("RemotePersonData", httpClient, "Data", "Count");

            var resultData = await dataTable.GetDataPageAsync(filterMock.Object, 1, 2);

            resultData.Should().NotBeNull();
            resultData.Should().BeEquivalentTo(
            [
                "data3",
            ]);
        }

        private static Mock<ITableFilter<TData>> SetupColumnFilterMock<TData, TColumn>(Expression<Func<TColumn, bool>> dataFilterExp, Mock<IColumn<TData, TColumn>> columnMock)
        {
            var columnFilterMock = new Mock<IColumnFilter<TData, TColumn>>();
            columnFilterMock.SetupGet(f => f.Column).Returns(columnMock.Object);
            columnFilterMock.SetupGet(f => f.Filter).Returns(dataFilterExp);
            columnFilterMock.Setup(f => f.Accept(It.IsAny<IColumnFilterVisitor<TData, FilterDto>>()))
                .Returns<IColumnFilterVisitor<TData, FilterDto>>(v => v.Visit(columnFilterMock.Object));

            var filterMock = new Mock<ITableFilter<TData>>();
            filterMock.SetupGet(f => f.ColumnFilters).Returns(new[] {
                columnFilterMock.Object,
            });
            return filterMock;
        }

        private static Mock<ITableFilter<TData>> SetupDataFilterMock<TData>(Expression<Func<TData, bool>> dataFilterExp)
        {
            var dataFilterMock = new Mock<IDataFilter<TData>>();
            dataFilterMock.SetupGet(f => f.DataFilter).Returns(dataFilterExp);

            var filterMock = new Mock<ITableFilter<TData>>();
            filterMock.SetupGet(f => f.DataFilters).Returns(new[] {
                dataFilterMock.Object,
            });
            return filterMock;
        }

        private static FilterDto SetupFilterDto<TColumn>(Expression<Func<TColumn, bool>> dataFilterExp, ColumnDto columnDto)
        {
            return new FilterDto()
            {
                Column = columnDto,
                FilterExpression = dataFilterExp.ToString(),
            };
        }

        private static FilterDto SetupFilterDto<TData>(Expression<Func<TData, bool>> dataFilterExp)
        {
            return new FilterDto()
            {
                FilterExpression = dataFilterExp.ToString(),
            };
        }

        private static Mock<IColumn<TData, TColumn>> SetupColumnMock<TData, TColumn>(string columnId, Expression<Func<TData, TColumn>> dataGetterExp)
        {
            var columnMock = new Mock<IColumn<TData, TColumn>>();
            columnMock.SetupGet(c => c.Id).Returns(columnId);
            columnMock.SetupGet(c => c.DataType).Returns(typeof(string));
            columnMock.SetupGet(c => c.DataGetterExpression).Returns(dataGetterExp);
            return columnMock;
        }

        private static ColumnDto SetupColumnDto<TData, TColumn>(string columnId, Expression<Func<TData, TColumn>> dataGetterExp)
        {
            return new ColumnDto()
            {
                Id = columnId,
                CanSort = false,
                CanFilter = false,
                DataType = typeof(string).AssemblyQualifiedName,
                DataGetterExpression = dataGetterExp.ToString(),
            };
        }
    }
}
