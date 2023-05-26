using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Deletions;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers;

public static class ServiceCollectionExtensions
{
    internal static void AddManufacturers(this IServiceCollection services)
    {
        services.AddTransient<IManufacturerFactory, ManufacturerFactory>();
        services.AddTransient<Func<CancellationToken, IManufacturerValidationService>>(provider =>
        {
            return ct => new ManufacturerValidationService(
                provider.GetRequiredService<Func<CancellationToken, IManufacturerRepository>>(),
                ct);
        });

        services.AddTransient<Func<CancellationToken, IManufacturerCreationService>>(provider =>
        {
            var manufacturerRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IManufacturerRepository>>();
            var manufacturerFactory = provider.GetRequiredService<IManufacturerFactory>();
            return cancellationToken => new ManufacturerCreationService(manufacturerRepositoryDelegate, manufacturerFactory,
                cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IManufacturerQueryService>>(provider =>
        {
            var manufacturerRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IManufacturerRepository>>();
            return cancellationToken => new ManufacturerQueryService(manufacturerRepositoryDelegate, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IManufacturerDeletionService>>(provider =>
        {
            var manufacturerRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IManufacturerRepository>>();
            var itemRepository = provider.GetRequiredService<IItemRepository>();
            return cancellationToken =>
                new ManufacturerDeletionService(manufacturerRepositoryDelegate, itemRepository, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IManufacturerModificationService>>(provider =>
        {
            var manufacturerRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IManufacturerRepository>>();
            return cancellationToken =>
                new ManufacturerModificationService(manufacturerRepositoryDelegate, cancellationToken);
        });
    }
}