using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Infrastructure.Specification;

internal static class SpecificationQueryBuilder
{
    public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, ISpecification<T>? specification)
        where T : class, IEntity
    {
        if(specification is null)
        {
            return inputQuery;
        }

        var query = inputQuery;

        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }
}
