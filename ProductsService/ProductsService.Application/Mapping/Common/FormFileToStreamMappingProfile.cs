using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ProductsService.Application.Mapping.Common;

internal class FormFileToStreamMappingProfile : Profile
{
    public FormFileToStreamMappingProfile()
    {
        CreateMap<IFormFile?, Stream?>()
            .ConvertUsing(formFile => formFile != null ? formFile.OpenReadStream() : null);
    }
}
