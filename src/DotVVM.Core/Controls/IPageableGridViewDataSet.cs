namespace DotVVM.Framework.Controls
{
    public interface IPageableGridViewDataSet : IGridViewDataSetBase
    {
        IPagingOptions PagingOptions { get; set; }
        void GoToFirstPage();
        void GoToLastPage();
        void GoToNextPage();
        void GoToPage(int pageNumber);
        void GoToPreviousPage();
    }
}