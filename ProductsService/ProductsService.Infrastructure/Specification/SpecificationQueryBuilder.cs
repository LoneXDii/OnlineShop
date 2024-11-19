using Microsoft.EntityFrameworkCore;
using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Infrastructure.Specification;

internal static class SpecificationQueryBuilder
{
    public static IQueryable<T> GetQuery<T>(IQueryable<T> inputQuery, BaseSpecification<T> specification)
        where T : class, IEntity
    {
        var query = inputQuery;

        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        return query;
    }
}
