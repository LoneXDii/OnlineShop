using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities.Abstractions;
using System.Linq.Expressions;

namespace ProductsService.Infrastructure.Specifications;

internal class Specification<T> : ISpecification<T> where T : IEntity
{
    public List<Expression<Func<T, bool>>> Criteries { get; } = new();
    public List<Expression<Func<T, object>>> Includes { get; } = new();
}
