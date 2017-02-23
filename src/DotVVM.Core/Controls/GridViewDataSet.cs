using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DotVVM.Framework.Controls
{
    public class GridViewDataSet<T> : IGridViewDataSet
    {
        public string SortExpression { get; set; }

        public bool SortDescending { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; }

        public int TotalItemsCount { get; set; }

        public IList<T> Items { get; set; }

        public string PrimaryKeyPropertyName { get; set; }

        public object EditRowId { get; set; }

        public virtual IList<int> NearPageNumbers
        {
            get {
                return Enumerable.Range(1, PagesCount)
                  .Where(n => Math.Abs(n - PageNumber) <= 5).ToList();
            }
        }

        public int PagesCount
        {
            get
            {
                if (TotalItemsCount == 0 || PageSize == 0)
                {
                    return 0;
                }
                return (int)Math.Ceiling((double)TotalItemsCount / PageSize);
            }
        }

        public bool IsFirstPage => PageNumber == 1;

        public bool IsLastPage => PageNumber == PagesCount;

        IList IDotVVMDataSet.Items => (IList)Items; 
        

        public GridViewDataSet()
        {
            Items = new List<T>();
        }

        public void GoToFirstPage()
        {
            PageNumber = 1;
        }

        public void GoToPreviousPage()
        {
            if (!IsFirstPage)
            {
                PageNumber--;
            }
        }

        public void GoToLastPage()
        {
            PageNumber = PagesCount;
        }

        public void GoToNextPage()
        {
            if (!IsLastPage)
            {
                PageNumber++;
            }
        }

        public void GoToPage(int number)
        {
            PageNumber = number;
        }

        public void Reset()
        {
            PageNumber = 1;
        }

        public virtual void SetSortExpression(string expression)
        {
            if (SortExpression == expression)
            {
                SortDescending = !SortDescending;
                GoToFirstPage();
            }
            else
            {
                SortExpression = expression;
                SortDescending = false;
                GoToFirstPage();
            }
        }

        public virtual void LoadFromQueryable(IQueryable<T> queryable)
        {
            TotalItemsCount = queryable.Count();

            if (!string.IsNullOrEmpty(SortExpression))
            {
                queryable = ApplySortExpression(queryable);
            }

            if (PageSize > 0)
            {
                queryable = queryable.Skip(PageSize * (PageNumber - 1)).Take(PageSize);
            }

            Items = queryable.ToList();
        }

        public virtual IQueryable<T> ApplySortExpression(IQueryable<T> queryable)
        {
            var type = typeof(T);
            var property = type.GetTypeInfo().GetProperty(SortExpression);

            if (property == null)
            {
                throw new Exception($"Could not sort by property '{SortExpression}', since it does not exists.");
            }

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderBy = Expression.Lambda(propertyAccess, parameter);

            var result = Expression.Call(typeof(Queryable),
                GetSortingMethodName(),
                new[] { type, property.PropertyType },
                queryable.Expression,
                Expression.Quote(orderBy));

            return queryable.Provider.CreateQuery<T>(result);
        }

        private string GetSortingMethodName()
        {
            return SortDescending ? "OrderByDescending" : "OrderBy";
        }
    }
}