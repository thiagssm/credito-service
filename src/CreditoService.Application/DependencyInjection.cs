using CreditoService.Application.DTOs.CreditoConstituido;
using CreditoService.Application.Services;
using CreditoService.Application.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CreditoService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICreditoApplicationService, CreditoApplicationService>();
        services.AddScoped<IValidator<CreditoConstituidoInputModel>, CreditoConstituidoInputModelValidator>();

        return services;
    }
}
