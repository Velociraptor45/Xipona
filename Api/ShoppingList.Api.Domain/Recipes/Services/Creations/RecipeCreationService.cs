using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

public class RecipeCreationService : IRecipeCreationService
{
    private readonly ILogger<IRecipeCreationService> _logger;
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRecipeFactory _recipeFactory;

    public RecipeCreationService(
        Func<CancellationToken, IRecipeRepository> recipeRepositoryDelegate,
        Func<CancellationToken, IRecipeFactory> recipeFactoryDelegate,
        ILogger<IRecipeCreationService> logger,
        CancellationToken cancellationToken)
    {
        _logger = logger;
        _recipeRepository = recipeRepositoryDelegate(cancellationToken);
        _recipeFactory = recipeFactoryDelegate(cancellationToken);
    }

    public async Task<IRecipe> CreateAsync(RecipeCreation creation)
    {
        var recipe = await _recipeFactory.CreateNewAsync(creation);
        var storedRecipe = await _recipeRepository.StoreAsync(recipe);

        _logger.LogInformation(() => $"Created recipe {storedRecipe.Name.Value}");

        return storedRecipe;
    }
}