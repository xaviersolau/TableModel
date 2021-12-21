// ----------------------------------------------------------------------
// <copyright file="IQueryableTableDataOptionsBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.TableModel.Options.Builder
{
#pragma warning disable CA1040 // Éviter les interfaces vides
    /// <summary>
    /// Queriable table data options builder interface.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TQueryableTableData">Queriable table data type.</typeparam>
    public interface IQueryableTableDataOptionsBuilder<TData, TQueryableTableData>
    {
    }
#pragma warning restore CA1040 // Éviter les interfaces vides
}