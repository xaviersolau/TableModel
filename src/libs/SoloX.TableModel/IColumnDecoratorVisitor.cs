// ----------------------------------------------------------------------
// <copyright file="IColumnDecoratorVisitor.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.TableModel
{
    /// <summary>
    /// Table Column Decorator Visitor interface.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TDecorator">Decorator type.</typeparam>
    public interface IColumnDecoratorVisitor<TData, TDecorator>
    {
        /// <summary>
        /// Visit the typed table data decorator.
        /// </summary>
        /// <typeparam name="TColumn">Column value type.</typeparam>
        /// <param name="columnDecorator">The typed column decorator.</param>
        void Visit<TColumn>(IColumnDecorator<TData, TDecorator, TColumn> columnDecorator);
    }

    /// <summary>
    /// Table Column Decorator Visitor interface returning a typed result.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TDecorator">Decorator value type.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    public interface IColumnDecoratorVisitor<TData, TDecorator, TResult>
    {
        /// <summary>
        /// Visit the typed table data decorator.
        /// </summary>
        /// <typeparam name="TColumn">Column value type.</typeparam>
        /// <param name="columnDecorator">The typed column decorator.</param>
        /// <returns>The visitor result.</returns>
        TResult Visit<TColumn>(IColumnDecorator<TData, TDecorator, TColumn> columnDecorator);
    }

    /// <summary>
    /// Table Column Decorator Visitor interface returning a typed result and taking a typed argument.
    /// </summary>
    /// <typeparam name="TData">Table data type.</typeparam>
    /// <typeparam name="TDecorator">Decorator value type.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <typeparam name="TArg">Type of the argument.</typeparam>
    public interface IColumnDecoratorVisitor<TData, TDecorator, TResult, TArg>
    {
        /// <summary>
        /// Visit the typed table data decorator.
        /// </summary>
        /// <typeparam name="TColumn">Column value type.</typeparam>
        /// <param name="columnDecorator">The typed column decorator.</param>
        /// <param name="arg">The visitor argument.</param>
        /// <returns>The visitor result.</returns>
        TResult Visit<TColumn>(IColumnDecorator<TData, TDecorator, TColumn> columnDecorator, TArg arg);
    }
}