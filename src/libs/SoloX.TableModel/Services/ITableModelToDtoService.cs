using SoloX.TableModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Services
{
    public interface ITableModelToDtoService
    {
        ColumnDto Map<TData>(IColumn<TData> column);
        ColumnDto Map<TData, TColumn>(IColumn<TData, TColumn> column);
        TableStructureDto Map<TData, TId>(ITableStructure<TData, TId> tableStructure);

        ColumnDecoratorDto Map<TData, TDecorator, TColumn>(IColumnDecorator<TData, TDecorator, TColumn> columnDecorator);
        ColumnDecoratorDto Map<TData, TDecorator>(IColumnDecorator<TData, TDecorator> columnDecorator);
        TableDecoratorDto Map<TData, TDecorator>(ITableDecorator<TData, TDecorator> tableDecorator);
    }
}
