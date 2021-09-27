using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    public class Column<TData, TColumn> : IColumn<TData, TColumn>
    {
        private readonly Func<TData, TColumn> dataGetter;
        public Column(string id, Expression<Func<TData, TColumn>> dataGetterExpression, bool canSort = true, bool canFilter = true)
        {
            Id = id;
            CanSort = canSort;
            CanFilter = canFilter;
            DataGetterExpression = dataGetterExpression;

            this.dataGetter = dataGetterExpression.Compile();
        }

        public Type DataType => typeof(TColumn);

        public string Id { get; private set; }

        public bool CanSort { get; private set; }

        public bool CanFilter { get; private set; }

        public Expression<Func<TData, TColumn>> DataGetterExpression { get; }

        public object GetObject(TData data)
        {
            return this.dataGetter(data);
        }

        public TColumn GetValue(TData data)
        {
            return this.dataGetter(data);
        }

        public void Accept(IColumnVisitor<TData> visitor)
        {
            visitor.Visit(this);
        }

        public TResult Accept<TResult>(IColumnVisitor<TData, TResult> visitor)
        {
            return visitor.Visit(this);
        }

        public TResult Accept<TResult, TArg>(IColumnVisitor<TData, TResult, TArg> visitor, TArg arg)
        {
            return visitor.Visit(this, arg);
        }
    }
}
