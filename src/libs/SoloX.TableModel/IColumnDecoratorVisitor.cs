namespace SoloX.TableModel
{
    public interface IColumnDecoratorVisitor<TData, TDecorator>
    {
        void Visit<TColumn>(IColumnDecorator<TData, TDecorator, TColumn> columnDecorator);
    }

    public interface IColumnDecoratorVisitor<TData, TDecorator, TResult>
    {
        TResult Visit<TColumn>(IColumnDecorator<TData, TDecorator, TColumn> columnDecorator);
    }
    
    public interface IColumnDecoratorVisitor<TData, TDecorator, TResult, TArg>
    {
        TResult Visit<TColumn>(IColumnDecorator<TData, TDecorator, TColumn> columnDecorator, TArg arg);
    }
}