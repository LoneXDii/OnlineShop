using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Domain.Common.Models;
using OrderService.Domain.Entities;

namespace OrderService.Application.Mapping;

internal class OrderListModelMappingProfile : Profile
{
    public OrderListModelMappingProfile()
    {
        CreateMap<PaginatedListModel<OrderEntity>, PaginatedListModel<OrderDTO>>();
    }
}

