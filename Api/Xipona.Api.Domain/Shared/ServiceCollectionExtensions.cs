using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Validations;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Validations;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Validations;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Services.Validations;
using ProjectHermes.Xipona.Api.Domain.Shared.Validations;

namespace ProjectHermes.Xipona.Api.Domain.Shared;

public static class ServiceCollectionExtensions
{
    internal static void AddShared(this IServiceCollection services)
    {
        services.AddTransient<Func<CancellationToken, IValidator>>(provider =>
        {
            var availabilityValidationServiceDelegate = provider
                .GetRequiredService<Func<CancellationToken, IAvailabilityValidationService>>();
            var itemCategoryValidationServiceDelegate = provider
                .GetRequiredService<Func<CancellationToken, IItemCategoryValidationService>>();
            var manufacturerValidationServiceDelegate = provider
                .GetRequiredService<Func<CancellationToken, IManufacturerValidationService>>();
            var itemValidationService = provider.GetRequiredService<Func<CancellationToken, IItemValidationService>>();
            var recipeTagValidationService = provider.GetRequiredService<Func<CancellationToken, IRecipeTagValidationService>>();
            return ct => new Validator(
                availabilityValidationServiceDelegate(ct),
                itemCategoryValidationServiceDelegate(ct),
                manufacturerValidationServiceDelegate(ct),
                itemValidationService(ct),
                recipeTagValidationService(ct));
        });
    }
}