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

        return p => p.Categories.Count(c => _attributeIds.Contains(c.Id)) == _attributeIds.Count();
    }
}
