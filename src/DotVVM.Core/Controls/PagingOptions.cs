using System;
using System.Collections.Generic;
using System.Linq;

namespace DotVVM.Framework.Controls
{
    public class PagingOptions : IPagingOptions
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }
        public int TotalItemsCount { get; set; }

        public int TotalPagesCount
        {
            get
            {
                if (TotalItemsCount == 0 || PageSize == 0)
                {
                    return 0;
                }
                return (int)Math.Ceiling(TotalItemsCount / (double)PageSize);
            }
        }

        public IList<int> NearPageNumbers
        {
            get
            {
                return Enumerable.Range(1, TotalPagesCount)
                    .Where(n => Math.Abs(n - PageNumber) <= 5).ToList();
            }
        }

        public bool IsFirstPage => PageNumber == 1;
        public bool IsLastPage => PageNumber == TotalPagesCount;
    }
}