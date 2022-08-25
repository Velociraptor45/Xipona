using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes;

internal static class ServiceCollectionExtensions
{
    internal static void AddRecipes(this IServiceCollection services)
    {
        services.AddTransient<IPreparationStepFactory, PreparationStepFactory>();

        services.AddTransient<Func<CancellationToken, IIngredientFactory>>(provider =>
        {
            var validator = provider.GetRequiredService<Func<CancellationToken, IValidator>>();
            return cancellationToken => new IngredientFactory(validator, cancellationToken);
        });

        services.AddTransient<Func<CancellationToken, IRecipeFactory>>(provider =>
        {
            var ingredientFactory = provider.GetRequiredService<Func<CancellationToken, IIngredientFactory>>();
            var preparationStepFactory = provider.GetRequiredService<IPreparationStepFactory>();
            return cancellationToken => new RecipeFactory(ingredientFactory, preparationStepFactory, cancellationToken);
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
            var logger = provider.GetRequiredService<ILogger<RecipeQueryService>>();
            return cancellationToken => new RecipeQueryService(repository, logger, cancellationToken);
        });
    }
}