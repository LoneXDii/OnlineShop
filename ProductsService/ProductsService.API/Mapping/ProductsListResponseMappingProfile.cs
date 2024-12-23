using AutoMapper;
using ProductsService.API.Protos;

namespace ProductsService.API.Mapping;

public class ProductsListResponseMappingProfile : Profile
{
    public ProductsListResponseMappingProfile()
    {
        //I can't create mapping from List<ProductEntity> to ProductsListResponse
        //because i need to map List<ProductEntity> into List<ProductResponse> first
        CreateMap<List<ProductResponse>, ProductsListResponse>()
            .AfterMap((src, dest) =>
            {
                dest.Products.AddRange(src);
            });
    }
}
