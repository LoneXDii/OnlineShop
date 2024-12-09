using ProductsService.Domain.Entities.Abstractions;
using System.Linq.Expressions;

namespace ProductsService.Domain.Abstractions.Specifications;

public interface ISpecification<T> where T : IEntity
{
    List<Expression<Func<T, bool>>> Criteries { get; }
    List<Expression<Func<T, object>>> Includes { get; }
}
