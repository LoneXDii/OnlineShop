using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Infrastructure.Specifications;

internal static class SpecificationExtension
{
    public static IQueryable<T> GetQuery<T>(this ISpecification<T> specification, IQueryable<T> inputQuery)
        where T : class, IEntity
    {
        if(specification is null)
        {
            return inputQuery;
        }

        var query = inputQuery;

        query = specification.Criteries.Aggregate(query, (current, criteria) => current.Where(criteria));
        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }
}
