using SoloX.TableModel.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoloX.TableModel.Services
{
    public interface IDtoToTableModelService
    {
        IColumn<TData> Map<TData>(ColumnDto dto);
        ITableStructure<TData> Map<TData>(TableStructureDto dto);
        ITableStructure<TData, TId> Map<TData, TId>(TableStructureDto dto);
        ITableDecorator<TData, TDecorator> Map<TData, TDecorator>(TableDecoratorDto dto, ITableStructure<TData> tableStructure);
        IColumnDecorator<TData, TDecorator, TColumn> Map<TData, TDecorator, TColumn>(ColumnDecoratorDto dto, IColumn<TData, TColumn> column);
    }
}
