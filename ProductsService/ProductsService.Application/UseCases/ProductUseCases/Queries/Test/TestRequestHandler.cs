using MediatR;
using ProductsService.Application.Specifications;
using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.Test;

internal class TestRequestHandler(IUnitOfWork unitOfWork) 
	: IRequestHandler<TestRequest, IEnumerable<Product>>
{
	public async Task<IEnumerable<Product>> Handle(TestRequest request, CancellationToken cancellationToken) 
	{
		var specification = new CombinableSpecification<Product>();
		specification = specification & new ProductCategorySpecification(2);
		specification = specification | new ProductPriceLessThanSpecification(1100);

		//specification.AddInclude(p => p.Category);
		//specification.AddInclude(p => p.ProductAttributes);
		specification.AddInclude(p => p.Attributes);

		var products = await unitOfWork.ProductQueryRepository.ListAsync(specification);

		return products;
	}
}
