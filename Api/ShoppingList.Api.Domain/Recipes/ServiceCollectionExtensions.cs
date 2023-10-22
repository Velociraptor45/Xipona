using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries.Quantities;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes;

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
            return ct => new RecipeConversionService(itemRepository(ct), itemCategoryRepository(ct));
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
            return ct => new RecipeFactory(ingredientFactory(ct), validator(ct), preparationStepFactory);
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