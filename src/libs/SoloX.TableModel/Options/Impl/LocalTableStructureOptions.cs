using SoloX.TableModel.Impl;
using SoloX.TableModel.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SoloX.TableModel.Options.Impl
{
    public class LocalTableStructureOptions<TData, TId> : ATableStructureOptions, ILocalTableStructureOptions<TData, TId>, ILocalTableStructureDataOptions<TData, TId>
    {
        private List<IColumn<TData>> dataColumns = new List<IColumn<TData>>();

        public IColumn<TData, TId> IdColumn { get; private set; }

        public IEnumerable<IColumn<TData>> DataColumns => dataColumns;

        public LocalTableStructureOptions(string tableStructureId, List<ATableDecoratorOptions> tableDecoratorOptions)
            : base(tableStructureId, tableDecoratorOptions)
        { }

        public ILocalTableStructureDataOptions<TData, TId> AddIdColumn(string columnId, Expression<Func<TData, TId>> idGetterExpression, bool canSort, bool canFilter)
        {
            if (IdColumn != null)
            {
                throw new InvalidDataException("Id column already defined");
            }

            IdColumn = new Column<TData, TId>(columnId, idGetterExpression, canSort, canFilter);

            return this;
        }

        public ILocalTableStructureDataOptions<TData, TId> AddColumn<TColumn>(string columnId, Expression<Func<TData, TColumn>> dataGetterExpression, bool canSort, bool canFilter)
        {
            this.dataColumns.Add(new Column<TData, TColumn>(columnId, dataGetterExpression, canSort, canFilter));

            return this;
        }

        public override Task<ITableStructure> CreateModelInstanceAsync(IServiceProvider serviceProvider)
        {
            var tableStructure = new TableStructure<TData, TId>(TableStructureId, IdColumn, DataColumns);

            return Task.FromResult<ITableStructure>(tableStructure);
        }
    }
}
