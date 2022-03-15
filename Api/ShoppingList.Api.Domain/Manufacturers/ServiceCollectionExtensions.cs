using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers;

public static class ServiceCollectionExtensions
{
    internal static void AddManufacturers(this IServiceCollection services)
    {
        services.AddTransient<IManufacturerFactory, ManufacturerFactory>();
        services.AddTransient<IManufacturerValidationService, ManufacturerValidationService>();

        services.AddTransient<Func<CancellationToken, IManufacturerCreationService>>(provider =>
        {
            var manufacturerRepository = provider.GetRequiredService<IManufacturerRepository>();
            var manufacturerFactory = provider.GetRequiredService<IManufacturerFactory>();
            return cancellationToken => new ManufacturerCreationService(manufacturerRepository, manufacturerFactory,
                cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IManufacturerQueryService>>(provider =>
        {
            var manufacturerRepository = provider.GetRequiredService<IManufacturerRepository>();
            return cancellationToken => new ManufacturerQueryService(manufacturerRepository, cancellationToken);
        });
    }
}