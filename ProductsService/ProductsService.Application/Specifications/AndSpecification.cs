using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities.Abstractions;
using System.Linq.Expressions;

namespace ProductsService.Application.Specifications;

internal class AndSpecification<T> : Specification<T> where T : IEntity
{
    private readonly ISpecification<T> _left;
    private readonly ISpecification<T> _right;

    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _right = right;
        _left = left;
    }

    public override Expression<Func<T, bool>> ToExpression()
    {
        var leftExpression = _left.ToExpression();
        var rightExpression = _right.ToExpression();

        var parameter = Expression.Parameter(typeof(T), nameof(T));
        var andExpression = Expression.AndAlso(
            Expression.Invoke(leftExpression, parameter),
            Expression.Invoke(rightExpression, parameter));

        return Expression.Lambda<Func<T, bool>>(andExpression, parameter);
    }
}
