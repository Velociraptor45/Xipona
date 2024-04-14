﻿using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Domain.Items.Ports;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models.Factories;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Ports;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Creations;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Deletions;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Modifications;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Validations;

namespace ProjectHermes.Xipona.Api.Domain.Manufacturers;

public static class ServiceCollectionExtensions
{
    internal static void AddManufacturers(this IServiceCollection services)
    {
        services.AddTransient<IManufacturerFactory, ManufacturerFactory>();
        services.AddTransient<Func<CancellationToken, IManufacturerValidationService>>(provider =>
        {
            return ct => new ManufacturerValidationService(
                provider.GetRequiredService<Func<CancellationToken, IManufacturerRepository>>()(ct));
        });

        services.AddTransient<Func<CancellationToken, IManufacturerCreationService>>(provider =>
        {
            var manufacturerRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IManufacturerRepository>>();
            var manufacturerFactory = provider.GetRequiredService<IManufacturerFactory>();
            return ct => new ManufacturerCreationService(manufacturerRepositoryDelegate(ct), manufacturerFactory);
        });

        services.AddTransient<Func<CancellationToken, IManufacturerQueryService>>(provider =>
        {
            var manufacturerRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IManufacturerRepository>>();
            return ct => new ManufacturerQueryService(manufacturerRepositoryDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IManufacturerDeletionService>>(provider =>
        {
            var manufacturerRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IManufacturerRepository>>();
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            return ct => new ManufacturerDeletionService(manufacturerRepositoryDelegate(ct), itemRepositoryDelegate(ct));
        });

        services.AddTransient<Func<CancellationToken, IManufacturerModificationService>>(provider =>
        {
            var manufacturerRepositoryDelegate = provider
                .GetRequiredService<Func<CancellationToken, IManufacturerRepository>>();
            return ct => new ManufacturerModificationService(manufacturerRepositoryDelegate(ct));
        });
    }
}