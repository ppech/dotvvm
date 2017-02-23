namespace DotVVM.Framework.Controls
{
    public interface IGridViewDataSetOptions
    {
        IPagingOptions PagingOptions { get; }
        ISortOptions SortOptions { get; }
    }
}