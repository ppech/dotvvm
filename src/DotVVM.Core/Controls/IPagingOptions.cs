using System.Collections.Generic;

namespace DotVVM.Framework.Controls
{
    public interface IPagingOptions
    {
        bool IsFirstPage { get; }
        bool IsLastPage { get; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        int TotalPagesCount { get; }
        int TotalItemsCount { get; set; }
        IList<int> NearPageNumbers { get; }
    }
}