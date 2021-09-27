using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Impl
{
    public class TableStructure<TData, TId> : ITableStructure<TData, TId>
    {
        private readonly IDictionary<string, IColumn<TData>> columnMap;

        public TableStructure(string id, IColumn<TData, TId> idColumn, params IColumn<TData>[] dataColumns)
        : this(id, idColumn, dataColumns.AsEnumerable<IColumn<TData>>())
        {
        }

        public TableStructure(string id, IColumn<TData, TId> idColumn, IEnumerable<IColumn<TData>> dataColumns)
        {
            Id = id;

            var columnList = new List<IColumn<TData>>();
            columnList.Add(idColumn);
            columnList.AddRange(dataColumns);

            Columns = columnList;
            IdColumn = idColumn;
            DataColumns = dataColumns;

            this.columnMap = new Dictionary<string, IColumn<TData>>();

            foreach (var col in columnList.Where(c => !string.IsNullOrEmpty(c.Id)))
            {
                this.columnMap.Add(col.Id, col);
            }
        }

        public string Id { get; }

        public IColumn<TData> this[string id] => this.columnMap[id];

        public IEnumerable<IColumn<TData>> Columns { get; private set; }

        public IEnumerable<IColumn<TData>> DataColumns { get; private set; }

        public IColumn<TData, TId> IdColumn { get; private set; }
    }
}
