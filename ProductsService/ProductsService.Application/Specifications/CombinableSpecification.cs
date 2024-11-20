using ProductsService.Domain.Abstractions.Specifications;
using ProductsService.Domain.Entities.Abstractions;
using System.Linq.Expressions;

namespace ProductsService.Application.Specifications;

internal class CombinableSpecification<T> : BaseSpecification<T> where T : IEntity
{
    public CombinableSpecification() 
        : base() { }

    protected CombinableSpecification(Expression<Func<T, bool>> criteria)
        : base(criteria) { }

    public static CombinableSpecification<T> operator &(CombinableSpecification<T> left, CombinableSpecification<T> right)
    {
        var leftExression = left.Criteria;
        var rightExression = right.Criteria;

        if (leftExression is null)
        {
            if(rightExression is null)
            {
                throw new NotImplementedException();
            }

            return right;
        }
        else if(rightExression is null)
        {
            return left;
        }

        var combinedParameters = leftExression.Parameters.Select(p => Expression.Parameter(typeof(T), p.Name)).ToList();

        var and = Expression.AndAlso(
            Expression.Invoke(leftExression, combinedParameters),
            Expression.Invoke(rightExression, combinedParameters)
        );

        var andExpr = Expression.Lambda<Func<T, bool>>(and, combinedParameters);

        return new CombinableSpecification<T>(andExpr);
    }

    public static CombinableSpecification<T> operator |(CombinableSpecification<T> left, CombinableSpecification<T> right)
    {
        var leftExression = left.Criteria;
        var rightExression = right.Criteria;

        if (leftExression is null)
        {
            if (rightExression is null)
            {
                throw new NotImplementedException();
            }

            return right;
        }
        else if (rightExression is null)
        {
            return left;
        }

        var combinedParameters = leftExression.Parameters.Select(p => Expression.Parameter(typeof(T), p.Name)).ToList();

        var or = Expression.OrElse(
            Expression.Invoke(leftExression, combinedParameters),
            Expression.Invoke(rightExression, combinedParameters)
        );

        var orExpr = Expression.Lambda<Func<T, bool>>(or, combinedParameters);

        return new CombinableSpecification<T>(orExpr);
    }
}
