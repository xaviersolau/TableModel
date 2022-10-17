// ----------------------------------------------------------------------
// <copyright file="DataFilterBase.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.ExpressionTools.Transform.Impl;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SoloX.TableModel.Impl
{
    /// <summary>
    /// Data filter descriptor.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    public class DataFilterBase<TData> : IDataFilter<TData>
    {
        /// <inheritdoc/>
        public Expression<Func<TData, bool>> DataFilter { get; }

        /// <summary>
        /// Setup a DataFilter.
        /// </summary>
        /// <param name="filter">Filter expression.</param>
        public DataFilterBase(Expression<Func<TData, bool>> filter)
            : this(filter, true)
        {
        }

        /// <summary>
        /// Setup a DataFilter.
        /// </summary>
        /// <param name="filter">Filter expression.</param>
        /// <param name="inline">Tells if the filter expression needs to be inlined.</param>
        protected DataFilterBase(Expression<Func<TData, bool>> filter, bool inline)
        {
            if (inline)
            {
                var constInliner = new ConstantInliner();
                DataFilter = constInliner.Amend(filter);
            }
            else
            {
                DataFilter = filter;
            }
        }

        /// <inheritdoc/>
        public IQueryable<TData> Apply(IQueryable<TData> data)
        {
            return data.Where(DataFilter);
        }
    }
}
