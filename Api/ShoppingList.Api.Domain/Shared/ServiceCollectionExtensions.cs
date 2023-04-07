using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

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
            var itemValidationService = provider.GetRequiredService<Func<CancellationToken, IItemValidationService>>();
            var recipeTagValidationService = provider.GetRequiredService<Func<CancellationToken, IRecipeTagValidationService>>();
            return cancellationToken => new Validator(
                availabilityValidationService,
                itemCategoryValidationService,
                manufacturerValidationService,
                itemValidationService,
                recipeTagValidationService,
                cancellationToken);
        });
    }
}