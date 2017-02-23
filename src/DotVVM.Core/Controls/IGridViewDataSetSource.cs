using System.Collections;

namespace DotVVM.Framework.Controls
{
    public interface IGridViewDataSetSource
    {
        IList Items { get; }
        int TotalItemsCount { get; }
    }
}