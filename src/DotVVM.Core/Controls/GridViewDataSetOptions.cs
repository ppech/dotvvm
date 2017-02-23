namespace DotVVM.Framework.Controls
{
    public class GridViewDataSetOptions : IGridViewDataSetOptions
    {
        public IPagingOptions PagingOptions { get; set; }

        public ISortOptions SortOptions { get; set; }
    }
}