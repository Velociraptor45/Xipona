using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Shared;

public static class ServiceCollectionExtensions
{
    internal static void AddShared(this IServiceCollection services)
    {
        services.AddTransient<Func<CancellationToken, IValidator>>(provider =>
        {
            var availabilityValidationService = provider.GetRequiredService<IAvailabilityValidationService>();
            var itemCategoryValidationService = provider.GetRequiredService<IItemCategoryValidationService>();
            var manufacturerValidationService = provider.GetRequiredService<IManufacturerValidationService>();
            return cancellationToken => new Validator(availabilityValidationService,
                itemCategoryValidationService, manufacturerValidationService, cancellationToken);
        });
    }
}