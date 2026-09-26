using AutoMapper;
using Swoms.Application.Features.Orders;
using Swoms.Application.Features.Products;
using Swoms.Application.Features.Warehouses;
using Swoms.Domain.Entities;

namespace Swoms.Application.Common.Mappings;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<Warehouse, WarehouseDto>()
            .ForMember(destination => destination.Line1, options => options.MapFrom(source => source.Address.Line1))
            .ForMember(destination => destination.City, options => options.MapFrom(source => source.Address.City))
            .ForMember(destination => destination.State, options => options.MapFrom(source => source.Address.State))
            .ForMember(destination => destination.PostalCode, options => options.MapFrom(source => source.Address.PostalCode))
            .ForMember(destination => destination.Country, options => options.MapFrom(source => source.Address.Country));
        CreateMap<Order, OrderDto>();
        CreateMap<OrderItem, OrderItemDto>();
    }
}
