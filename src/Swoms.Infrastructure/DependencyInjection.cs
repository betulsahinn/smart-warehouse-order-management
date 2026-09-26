using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swoms.Application.Common.Interfaces;
using Swoms.Application.Features.Auth;
using Swoms.Infrastructure.Persistence;
using Swoms.Infrastructure.Persistence.Repositories;
using Swoms.Infrastructure.Security;
using Swoms.Infrastructure.Services;

namespace Swoms.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' must be configured.");
        }

        services.AddDbContext<SwomsDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<SwomsDbContext>());
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
