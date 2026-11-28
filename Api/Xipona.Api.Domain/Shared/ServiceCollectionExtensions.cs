using Microsoft.Extensions.DependencyInjection;
using Xipona.Api.Domain.ItemCategories.Services.Validations;
using Xipona.Api.Domain.Items.Services.Validations;
using Xipona.Api.Domain.Manufacturers.Services.Validations;
using Xipona.Api.Domain.RecipeTags.Services.Validations;
using Xipona.Api.Domain.Shared.Validations;

namespace Xipona.Api.Domain.Shared;

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