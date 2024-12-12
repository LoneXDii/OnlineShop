using ProductsService.Domain.Entities;
using System.Linq.Expressions;

namespace ProductsService.Application.Specifications.Products;

internal class ProductAttributesSpecification : Specification<Product>
{
    private readonly int[]? _attributeIds;

    public ProductAttributesSpecification(int[]? AttributeIds)
    {
        _attributeIds = AttributeIds;
    }

    public override Expression<Func<Product, bool>> ToExpression()
    {
        if (_attributeIds is null) 
        {
            return product => true;
        }

        Expression<Func<Product, bool>> expression = product => true;

        foreach (var attributeId in _attributeIds)
        {
            Expression<Func<Product, bool>> rightExpression = product => product.Categories.Any(c => c.Id == attributeId);
            var parameter = Expression.Parameter(typeof(Product), nameof(Product));

            var binaryExpression = Expression.AndAlso(
                Expression.Invoke(expression, parameter),
                Expression.Invoke(rightExpression, parameter));

            expression = Expression.Lambda<Func<Product, bool>>(binaryExpression, parameter);
        }

        return expression;
    }
}
