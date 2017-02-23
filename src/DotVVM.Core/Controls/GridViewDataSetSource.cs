using System.Collections;
using System.Collections.Generic;

namespace DotVVM.Framework.Controls
{
    public class GridViewDataSetSource<T> : IGridViewDataSetSource
    {
        public IList<T> Items { get; set; }
        IList IGridViewDataSetSource.Items => (IList)Items;
        public int TotalItemsCount { get; set; }
    }
}