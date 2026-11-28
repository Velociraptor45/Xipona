using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xipona.Api.Core.Services;
using Xipona.Api.Domain.ItemCategories.Ports;
using Xipona.Api.Domain.Items.Ports;
using Xipona.Api.Domain.Recipes.Models.Factories;
using Xipona.Api.Domain.Recipes.Ports;
using Xipona.Api.Domain.Recipes.Services.Creations;
using Xipona.Api.Domain.Recipes.Services.Modifications;
using Xipona.Api.Domain.Recipes.Services.Queries;
using Xipona.Api.Domain.Recipes.Services.Queries.Quantities;
using Xipona.Api.Domain.Recipes.Services.Shared;
using Xipona.Api.Domain.Shared.Validations;
using Xipona.Api.Domain.Stores.Ports;

namespace Xipona.Api.Domain.Recipes;

internal static class ServiceCollectionExtensions
{
    internal static void AddRecipes(this IServiceCollection services)
    {
        services.AddTransient<IPreparationStepFactory, PreparationStepFactory>();
        services.AddTransient<IQuantitiesQueryService, QuantitiesQueryService>();

        services.AddTransient<Func<CancellationToken, IRecipeConversionService>>(provider =>
        {
            var itemRepository = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            var itemCategoryRepository = provider.GetRequiredService<Func<CancellationToken, IItemCategoryRepository>>();
            var recipeReadRepo = provider.GetRequiredService<Func<CancellationToken, IRecipeReadRepository>>();
            return ct => new RecipeConversionService(itemRepository(ct), itemCategoryRepository(ct), recipeReadRepo(ct));
        });

        services.AddTransient<Func<CancellationToken, IIngredientFactory>>(provider =>
        {
            var validator = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            return ct => new IngredientFactory(validator(ct));
        });

        services.AddTransient<Func<CancellationToken, IRecipeFactory>>(provider =>
        {
            var ingredientFactory = provider.GetRequiredService<Func<CancellationToken, IIngredientFactory>>();
            var validator = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            var preparationStepFactory = provider.GetRequiredService<IPreparationStepFactory>();
            var dateTimeService = provider.GetRequiredService<IDateTimeService>();
            return ct => new RecipeFactory(ingredientFactory(ct), validator(ct), preparationStepFactory,
                dateTimeService);
        });

        services.AddTransient<Func<CancellationToken, IRecipeCreationService>>(provider =>
        {
            var recipeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IRecipeRepository>>();
            var recipeFactoryDelegate = provider.GetRequiredService<Func<CancellationToken, IRecipeFactory>>();
            var conversionServiceDelegate = provider.GetRequiredService<Func<CancellationToken, IRecipeConversionService>>();
            var logger = provider.GetRequiredService<ILogger<RecipeCreationService>>();
            return ct => new RecipeCreationService(recipeRepositoryDelegate(ct), recipeFactoryDelegate(ct),
                conversionServiceDelegate(ct), logger);
        });
        services.AddTransient<Func<CancellationToken, IRecipeQueryService>>(provider =>
        {
            var repository = provider.GetRequiredService<Func<CancellationToken, IRecipeRepository>>();
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            var conversionServiceDelegate = provider.GetRequiredService<Func<CancellationToken, IRecipeConversionService>>();
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            var translationService = provider.GetRequiredService<IQuantityTranslationService>();
            var logger = provider.GetRequiredService<ILogger<RecipeQueryService>>();
            return ct => new RecipeQueryService(repository(ct), itemRepositoryDelegate(ct),
                conversionServiceDelegate(ct), storeRepositoryDelegate(ct), translationService, logger);
        });
        services.AddTransient<Func<CancellationToken, IRecipeModificationService>>(provider =>
        {
            var recipeRepository = provider.GetRequiredService<Func<CancellationToken, IRecipeRepository>>();
            var itemRepository = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            var validator = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            return ct => new RecipeModificationService(recipeRepository(ct), itemRepository(ct), validator(ct));
        });

        services.AddTransient<IQuantityTranslationService, QuantityTranslationService>();
    }
}