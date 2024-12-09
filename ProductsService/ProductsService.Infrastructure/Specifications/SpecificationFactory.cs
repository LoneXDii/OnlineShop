using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Infrastructure.Specifications;

internal class SpecificationFactory : ISpecificationFactory
{
    public ISpecification<T> CreateSpecification<T>() where T : IEntity
    {
        return new Specification<T>();
    }
}
