using CreditoService.Application.Abstractions.Persistence;
using CreditoService.Infrastructure.Persistence;
using CreditoService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CreditoService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string DefaultConnection nao configurada.");

        services.AddDbContext<CreditoDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<ICreditoRepository, CreditoRepository>();

        return services;
    }
}
