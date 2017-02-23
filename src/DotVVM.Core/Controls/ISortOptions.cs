namespace DotVVM.Framework.Controls
{
    public interface ISortOptions
    {
        string PrimaryKeyPropertyName { get; set; }
        string SortExpression { get; set; }
        bool SortDescending { get; set; }
    }
}