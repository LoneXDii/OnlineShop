using AutoMapper;
using MediatR;
using ProductsService.Application.DTO;
using ProductsService.Application.Specifications;
using ProductsService.Application.Specifications.Products;
using ProductsService.Domain.Abstractions.Database;
using ProductsService.Domain.Entities;

namespace ProductsService.Application.UseCases.ProductUseCases.Queries.Test;

internal class TestRequestHandler(IUnitOfWork unitOfWork, IMapper mapper) 
	: IRequestHandler<TestRequest, IEnumerable<ProductDTO>>
{
	public async Task<IEnumerable<ProductDTO>> Handle(TestRequest request, CancellationToken cancellationToken) 
	{
		var specification = new CombinableSpecification<Product>();
		specification = specification & new ProductCategorySpecification(2);
		specification = specification & new ProductPriceLessThanSpecification(1100);

		var products = await unitOfWork.ProductQueryRepository.ListAsync(specification);

		throw new NotImplementedException();
		//var ret = mapper.Map<List<ProductDTO>>(products.ToList());

		//return ret;
	}
}
