using SoloX.ExpressionTools.Transform.Impl.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    public class TableSorting<TData> : ITableSorting<TData>
    {
        private List<IColumnSorting<TData>> columnSortings = new List<IColumnSorting<TData>>();

        public IEnumerable<IColumnSorting<TData>> ColumnSortings => columnSortings;

        public IQueryable<TData> Apply(IQueryable<TData> data)
        {
            var dataIter = data;

            foreach (var columnSorting in columnSortings)
            {
                dataIter = columnSorting.Apply(dataIter);
            }

            return dataIter;
        }

        public void Register<TColumn>(Expression<Func<TData, TColumn>> data, SortingOrder order)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var propertyNameResolver = new PropertyNameResolver();

            var id = propertyNameResolver.GetPropertyName(data);

            Register(id, data, order);
        }

        public void Register<TColumn>(string columnId, Expression<Func<TData, TColumn>> data, SortingOrder order)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Register(new Column<TData, TColumn>(columnId, data), order);
        }

        public void Register(IColumn<TData> column, SortingOrder order)
        {
            column.Accept(new Visitor(order, this.columnSortings));
        }

        public void UnRegister(IColumn<TData> column)
        {
            throw new NotImplementedException();
        }

        private class Visitor : IColumnVisitor<TData>
        {
            private SortingOrder order;
            private List<IColumnSorting<TData>> columnSortings;

            public Visitor(SortingOrder order, List<IColumnSorting<TData>> columnSortings)
            {
                this.order = order;
                this.columnSortings = columnSortings;
            }

            public void Visit<TColumn>(IColumn<TData, TColumn> column)
            {
                columnSortings.Add(new ColumnSorting<TData, TColumn>(column, order));
            }
        }
    }
}
