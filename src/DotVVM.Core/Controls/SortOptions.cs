namespace DotVVM.Framework.Controls
{
    class SortOptions : ISortOptions
    {
        public string PrimaryKeyPropertyName { get; set; }
        public string SortExpression { get; set; }
        public bool SortDescending { get; set; }
    }
}