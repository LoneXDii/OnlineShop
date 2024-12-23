using ProductsService.Domain.Entities.Abstractions;
using System.Linq.Expressions;

namespace ProductsService.Domain.Abstractions.Specifications;

public interface ISpecification<T> where T : IEntity
{
    bool IsSatisfiedBy(T candidate);
    Expression<Func<T, bool>> ToExpression();
    ISpecification<T> And(ISpecification<T> other);
}
