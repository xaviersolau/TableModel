// ----------------------------------------------------------------------
// <copyright file="TableFactoryTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.TableModel.Impl;
using SoloX.TableModel.UTests.Samples;
using Xunit;

namespace SoloX.TableModel.UTests
{
    public class TableFactoryTest
    {
        [Fact]
        public void ItShouldCreateATableFilter()
        {
            var factory = new TableFactory();

            var tableFilter = factory.CreateTableFilter<FamilyMemberDto>();

            Assert.NotNull(tableFilter);
        }

        [Fact]
        public void ItShouldCreateATableSorting()
        {
            var factory = new TableFactory();

            var tableSorting = factory.CreateTableSorting<FamilyMemberDto>();

            Assert.NotNull(tableSorting);
        }
    }
}
