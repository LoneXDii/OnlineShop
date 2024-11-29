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
            return right.CombineIncludes(left);
        }
        else if (rightExression is null)
        {
            return left.CombineIncludes(right);
        }

        var combinedParameters = leftExression.Parameters.Select(p => Expression.Parameter(typeof(T), p.Name)).ToList();

        var and = Expression.AndAlso(
            Expression.Invoke(leftExression, combinedParameters),
            Expression.Invoke(rightExression, combinedParameters)
        );

        var andExpr = Expression.Lambda<Func<T, bool>>(and, combinedParameters);
        var specification = new CombinableSpecification<T>(andExpr);
        specification.CombineIncludes(left);
        specification.CombineIncludes(right);

        return specification;
    }

    public static CombinableSpecification<T> operator |(CombinableSpecification<T> left, CombinableSpecification<T> right)
    {
        var leftExression = left.Criteria;
        var rightExression = right.Criteria;

        if (leftExression is null)
        {
            return right.CombineIncludes(left);
        }
        else if (rightExression is null)
        {
            return left.CombineIncludes(right);
        }

        var combinedParameters = leftExression.Parameters.Select(p => Expression.Parameter(typeof(T), p.Name)).ToList();

        var or = Expression.OrElse(
            Expression.Invoke(leftExression, combinedParameters),
            Expression.Invoke(rightExression, combinedParameters)
        );

        var orExpr = Expression.Lambda<Func<T, bool>>(or, combinedParameters);
        var specification = new CombinableSpecification<T>(orExpr);    
        specification.CombineIncludes(left);
        specification.CombineIncludes(right);

        return specification;
    }

    private CombinableSpecification<T> CombineIncludes(CombinableSpecification<T> other)
    {
        Includes.AddRange(other.Includes);

        return this;
    }
}
