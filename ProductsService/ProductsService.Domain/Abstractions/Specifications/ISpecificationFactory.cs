using ProductsService.Domain.Entities.Abstractions;

namespace ProductsService.Domain.Abstractions.Specifications;

public interface ISpecificationFactory
{
    ISpecification<T> CreateSpecification<T>() where T : IEntity;
}
