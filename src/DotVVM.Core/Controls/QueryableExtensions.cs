using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DotVVM.Framework.Controls
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyGridViewDataSetOptions<T>(this IQueryable<T> query, IGridViewDataSetOptions gridViewDataSetOptions)
        {
            return query.ApplySortOptions(gridViewDataSetOptions.SortOptions).ApplyPagingOptions(gridViewDataSetOptions.PagingOptions);
        }

        public static IQueryable<T> ApplySortOptions<T>(this IQueryable<T> query, IGridViewDataSetOptions gridViewDataSetOptions)
        {
            return query.ApplySortOptions(gridViewDataSetOptions.SortOptions);
        }

        public static IQueryable<T> ApplyPagingOptions<T>(this IQueryable<T> query, IGridViewDataSetOptions gridViewDataSetOptions)
        {
            return query.ApplyPagingOptions(gridViewDataSetOptions.PagingOptions);
        }

        public static IQueryable<T> ApplyPagingOptions<T>(this IQueryable<T> query, IPagingOptions pagingOptions)
        {
            return query.Skip((pagingOptions.PageNumber - 1) * pagingOptions.PageSize)
                .Take(pagingOptions.PageSize);
        }


        public static IQueryable<T> ApplySortOptions<T>(this IQueryable<T> query, ISortOptions sortOptions)
        {
            if (!string.IsNullOrEmpty(sortOptions.SortExpression))
            {
                var type = typeof(T);
                var property = type.GetTypeInfo().GetProperty(sortOptions.SortExpression);
                if (property == null)
                {
                    throw new Exception($"Could not sort by property '{sortOptions.SortExpression}', since it does not exists.");
                }
                var parameterExpression = Expression.Parameter(type, "p");
                var lambdaExpression = Expression.Lambda(Expression.MakeMemberAccess(parameterExpression, property), parameterExpression);
                var methodCallExpression = Expression.Call(typeof(Queryable),
                                            GetSortingMethodName(sortOptions.SortDescending),
                                            new Type[2] {
                                                type,
                                                property.PropertyType
                                            },
                                            query.Expression,
                                            Expression.Quote(lambdaExpression));

                return query.Provider.CreateQuery<T>(methodCallExpression);
            }
            return query;
        }

        private static string GetSortingMethodName(bool sortDescending)
        {
            return sortDescending ? "OrderByDescending" : "OrderBy";
        }
    }
}