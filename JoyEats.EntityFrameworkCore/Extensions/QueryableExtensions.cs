using System.Linq.Expressions;

namespace JoyEats.EntityFrameworkCore.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIF<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }
    }
}
