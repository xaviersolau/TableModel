// ----------------------------------------------------------------------
// <copyright file="LocalTableDecoratorOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.ExpressionTools.Transform.Impl.Resolver;
using SoloX.TableModel.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SoloX.TableModel.Options.Impl
{
    /// <summary>
    /// Local table decorator options.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TId">Table Id type.</typeparam>
    /// <typeparam name="TDecorator">Decorator value type.</typeparam>
    public class LocalTableDecoratorOptions<TData, TId, TDecorator> : ATableDecoratorOptions, ILocalTableDecoratorOptions<TData, TDecorator>, ILocalTableDecoratorDataOptions<TData, TDecorator>
    {
        /// <summary>
        /// Get Default decorator expression (from column value).
        /// </summary>
        public Expression<Func<object, TDecorator>> DefaultDecoratorExpression { get; private set; }

        /// <summary>
        /// Get Default header decorator expression.
        /// </summary>
        public Expression<Func<IColumn<TData>, TDecorator>> DefaultHeaderDecoratorExpression { get; private set; }

        private readonly Dictionary<string, Action<TableDecorator<TData, TDecorator>>> columnDecoratorRegisterActions = new Dictionary<string, Action<TableDecorator<TData, TDecorator>>>();

        private IReadOnlyDictionary<string, Action<TableDecorator<TData, TDecorator>>> ColumnDecoratorRegisterActions => this.columnDecoratorRegisterActions;

        /// <summary>
        /// Setup Local table decorator options with Ids.
        /// </summary>
        /// <param name="tableStructureId">Table structure Id.</param>
        /// <param name="tableDecoratorId">Table decorator Id.</param>
        public LocalTableDecoratorOptions(string tableStructureId, string tableDecoratorId)
            : base(tableStructureId, tableDecoratorId)
        {
        }

        /// <inheritdoc/>
        public ILocalTableDecoratorDataOptions<TData, TDecorator> AddDefault(
            Expression<Func<object, TDecorator>> defaultDecoratorExpression,
            Expression<Func<IColumn<TData>, TDecorator>> defaultHeaderDecoratorExpression)
        {
            if (DefaultDecoratorExpression != null)
            {
                throw new InvalidDataException("Default decorator expression already defined");
            }

            DefaultDecoratorExpression = defaultDecoratorExpression;

            if (DefaultHeaderDecoratorExpression != null)
            {
                throw new InvalidDataException("Default header decorator expression already defined");
            }

            DefaultHeaderDecoratorExpression = defaultHeaderDecoratorExpression;

            return this;
        }

        /// <inheritdoc/>
        public ILocalTableDecoratorDataOptions<TData, TDecorator> Add<TColumn>(
            string columnId,
            Expression<Func<TColumn, TDecorator>> decoratorExpression,
            Expression<Func<TDecorator>>? headerDecoratorExpression)
        {
            if (this.columnDecoratorRegisterActions.ContainsKey(columnId))
            {
                throw new InvalidDataException($"Decorator expression already defined fro {columnId}");
            }

            this.columnDecoratorRegisterActions.Add(columnId, tableDecorator =>
            {
                var registered = tableDecorator.TryRegister(columnId, decoratorExpression, headerDecoratorExpression);

                // TODO log warning if not registered.
            });

            return this;
        }

        /// <inheritdoc/>
        public ILocalTableDecoratorDataOptions<TData, TDecorator> Add<TColumn>(
            Expression<Func<TData, TColumn>> columnPropertyExpression,
            Expression<Func<TColumn, TDecorator>> decoratorExpression,
            Expression<Func<TDecorator>>? headerDecoratorExpression)
        {
            var propertyNameResolver = new PropertyNameResolver();
            return Add(propertyNameResolver.GetPropertyName(columnPropertyExpression), decoratorExpression, headerDecoratorExpression);
        }

        /// <inheritdoc/>
        public override async Task<ITableDecorator> CreateModelInstanceAsync(IServiceProvider serviceProvider, ITableStructureRepository tableStructureRepository)
        {
            if (tableStructureRepository == null)
            {
                throw new ArgumentNullException(nameof(tableStructureRepository));
            }

            var tableStructure = await tableStructureRepository
                .GetTableStructureAsync<TData>(TableStructureId)
                .ConfigureAwait(false);

            var tableDecorator = new TableDecorator<TData, TDecorator>(TableDecoratorId, tableStructure);

            tableDecorator.RegisterDefault(DefaultDecoratorExpression, DefaultHeaderDecoratorExpression);

            foreach (var setupAction in ColumnDecoratorRegisterActions)
            {
                setupAction.Value(tableDecorator);
            }

            return tableDecorator;
        }
    }
}
