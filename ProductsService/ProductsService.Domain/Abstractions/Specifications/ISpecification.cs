using ProductsService.Domain.Entities.Abstractions;
using System.Linq.Expressions;

namespace ProductsService.Domain.Abstractions.Specifications;

public interface ISpecification<T> where T : IEntity
{
    Expression<Func<T, bool>>? Criteria { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<string> IncludeStrings { get; }
    Expression<Func<T, object>>? DescendingBy { get; }
}
