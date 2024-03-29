﻿// ----------------------------------------------------------------------
// <copyright file="TableFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// Table Factory implementation.
    /// </summary>
    public class TableFactory : ITableFactory
    {
        /// <inheritdoc/>
        public ITableFilter<TData> CreateTableFilter<TData>()
        {
            return new TableFilter<TData>();
        }

        /// <inheritdoc/>
        public ITableSorting<TData> CreateTableSorting<TData>()
        {
            return new TableSorting<TData>();
        }
    }
}
