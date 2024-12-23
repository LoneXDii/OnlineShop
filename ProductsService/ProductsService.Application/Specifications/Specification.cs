using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities.Abstractions;
using System.Linq.Expressions;

namespace ProductsService.Application.Specifications;

internal abstract class Specification<T> : ISpecification<T> where T : IEntity
{
    public bool IsSatisfiedBy(T candidate)
    {
        Func<T, bool> predicate = ToExpression().Compile();

        return predicate(candidate);
    }

    public ISpecification<T> And(ISpecification<T> other)
    {
        return new AndSpecification<T>(this, other);
    }

    public abstract Expression<Func<T, bool>> ToExpression();
}
