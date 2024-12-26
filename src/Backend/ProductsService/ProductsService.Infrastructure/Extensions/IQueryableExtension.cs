using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ProductsService.Infrastructure.Extensions;

internal static class IQueryableExtension
{
    public static IQueryable<T> Include<T>(this IQueryable<T> query, Expression<Func<T, object>>[] includedProperties) where T : class
    {
        foreach (var property in includedProperties)
        {
            query = query.Include(property);
        }

        return query;
    }
}
