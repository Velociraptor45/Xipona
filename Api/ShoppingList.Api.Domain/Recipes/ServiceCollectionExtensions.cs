using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries.Quantities;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes;

internal static class ServiceCollectionExtensions
{
    internal static void AddRecipes(this IServiceCollection services)
    {
        services.AddTransient<IPreparationStepFactory, PreparationStepFactory>();
        services.AddTransient<IQuantitiesQueryService, QuantitiesQueryService>();

        services.AddTransient<Func<CancellationToken, IIngredientFactory>>(provider =>
        {
            var validator = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            return cancellationToken => new IngredientFactory(validator, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IRecipeFactory>>(provider =>
        {
            var ingredientFactory = provider.GetRequiredService<Func<CancellationToken, IIngredientFactory>>();
            var validator = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            var preparationStepFactory = provider.GetRequiredService<IPreparationStepFactory>();
            return cancellationToken =>
                new RecipeFactory(ingredientFactory, validator, preparationStepFactory, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IRecipeCreationService>>(provider =>
        {
            var recipeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IRecipeRepository>>();
            var recipeFactoryDelegate = provider.GetRequiredService<Func<CancellationToken, IRecipeFactory>>();
            var logger = provider.GetRequiredService<ILogger<RecipeCreationService>>();
            return cancellationToken =>
                new RecipeCreationService(recipeRepositoryDelegate, recipeFactoryDelegate, logger, cancellationToken);
        });
        services.AddTransient<Func<CancellationToken, IRecipeQueryService>>(provider =>
        {
            var repository = provider.GetRequiredService<Func<CancellationToken, IRecipeRepository>>();
            var itemRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IItemRepository>>();
            var storeRepositoryDelegate = provider.GetRequiredService<Func<CancellationToken, IStoreRepository>>();
            var translationService = provider.GetRequiredService<IQuantityTranslationService>();
            var logger = provider.GetRequiredService<ILogger<RecipeQueryService>>();
            return cancellationToken => new RecipeQueryService(repository, itemRepositoryDelegate, storeRepositoryDelegate,
                translationService, logger, cancellationToken);
        });
        services.AddTransient<Func<CancellationToken, IRecipeModificationService>>(provider =>
        {
            var repository = provider.GetRequiredService<Func<CancellationToken, IRecipeRepository>>();
            var validator = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            return cancellationToken => new RecipeModificationService(repository, validator, cancellationToken);
        });

        services.AddTransient<IQuantityTranslationService, QuantityTranslationService>();
    }
}