using System.Linq;

namespace DotVVM.Framework.Controls
{
    public delegate IGridViewDataSetSource LoadDataByOptionsDelegate(IGridViewDataSetOptions gridViewDataSetOptions);

    public interface IGridViewDataSetBase : IDotVVMDataSet
    {
        LoadDataByOptionsDelegate LoadData { get; }
        object EditRowId { get; set; }
        bool IsRefreshRequired { get; }
        ISortOptions SortOptions { get; }
        void ReloadData();
        void LoadFromQueryable(IQueryable queryable);
        void SetSortExpression(string expression);
    }
}