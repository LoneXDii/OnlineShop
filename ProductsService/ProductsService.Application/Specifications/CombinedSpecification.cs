using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities.Abstractions;
using System.Linq.Expressions;

namespace ProductsService.Application.Specifications;

internal class CombinedSpecification<T> : BaseSpecification<T> where T : IEntity
{
    public CombinedSpecification(Expression<Func<T, bool>> criteria)
        : base(criteria) { }

    public static CombinedSpecification<T> operator &(CombinedSpecification<T> left, CombinedSpecification<T> right)
    {
        var leftExression = left.Criteria;
        var rightExression = right.Criteria;

        if (leftExression is null)
        {
            if(rightExression is null)
            {
                throw new NotImplementedException();
            }

            return new CombinedSpecification<T>(rightExression);
        }
        else if(rightExression is null)
        {
            return new CombinedSpecification<T>(leftExression);
        }

        var and = Expression.AndAlso(leftExression.Body, rightExression.Body);
        var andExpr = Expression.Lambda<Func<T, bool>>(and, leftExression.Parameters.Single());

        return new CombinedSpecification<T>(andExpr);
    }

    public static CombinedSpecification<T> operator |(CombinedSpecification<T> left, CombinedSpecification<T> right)
    {
        var leftExression = left.Criteria;
        var rightExression = right.Criteria;

        if (leftExression is null)
        {
            if (rightExression is null)
            {
                throw new NotImplementedException();
            }

            return new CombinedSpecification<T>(rightExression);
        }
        else if (rightExression is null)
        {
            return new CombinedSpecification<T>(leftExression);
        }

        var or = Expression.OrElse(leftExression.Body, rightExression.Body);
        var orExpr = Expression.Lambda<Func<T, bool>>(or, leftExression.Parameters.Single());

        return new CombinedSpecification<T>(orExpr);
    }
}
