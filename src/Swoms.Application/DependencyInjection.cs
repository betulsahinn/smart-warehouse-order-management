using System.Reflection;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swoms.Application.Features.Orders;
using Swoms.Application.Features.Products;
using Swoms.Application.Features.Warehouses;

namespace Swoms.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddSingleton(provider => new MapperConfiguration(
            configuration => configuration.AddMaps(assembly),
            provider.GetRequiredService<ILoggerFactory>()));
        services.AddSingleton<IMapper>(provider =>
            provider.GetRequiredService<MapperConfiguration>().CreateMapper(provider.GetService));
        services.AddValidatorsFromAssembly(assembly);

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
