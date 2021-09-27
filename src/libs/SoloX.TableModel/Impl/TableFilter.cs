using SoloX.ExpressionTools.Transform.Impl;
using SoloX.ExpressionTools.Transform.Impl.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    public class TableFilter<TData> : ITableFilter<TData>
    {
        private List<IColumnFilter<TData>> columnFilters = new List<IColumnFilter<TData>>();

        public IEnumerable<IColumnFilter<TData>> ColumnFilters => this.columnFilters;

        public IQueryable<TData> Apply(IQueryable<TData> data)
        {
            var dataIter = data;

            foreach (var columnFilter in columnFilters)
            {
                dataIter = columnFilter.Apply(dataIter);
            }

            return dataIter;
        }

        public void Register<TColumn>(Expression<Func<TData, TColumn>> data, Expression<Func<TColumn, bool>> filter)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var propertyNameResolver = new PropertyNameResolver();

            var id = propertyNameResolver.GetPropertyName(data);

            Register(id, data, filter);
        }

        public void Register<TColumn>(string columnId, Expression<Func<TData, TColumn>> data, Expression<Func<TColumn, bool>> filter)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Register(new Column<TData,TColumn>(columnId, data), filter);
        }

        public void Register<TColumn>(IColumn<TData> column, Expression<Func<TColumn, bool>> filter)
        {
            if (column is IColumn<TData, TColumn> typedColumn)
            {
                Register(typedColumn, filter);
            }
            else
            {
                throw new ArgumentException($"Incompatible column type {column.Id} [Type: {typeof(TColumn).Name}]");
            }
        }

        public void Register<TColumn>(IColumn<TData, TColumn> column, Expression<Func<TColumn, bool>> filter)
        {
            this.columnFilters.Add(new ColumnFilter<TData, TColumn>(column, filter));
        }

        public void UnRegister(IColumn<TData> column)
        {
            throw new NotImplementedException();
        }

        public void UnRegister(string columnId)
        {
            throw new NotImplementedException();
        }
    }
}
