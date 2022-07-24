using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

public class RecipeCreationService : IRecipeCreationService
{
    private readonly IRecipeRepository _recipeRepository;
    private readonly IRecipeFactory _recipeFactory;

    public RecipeCreationService(
        Func<CancellationToken, IRecipeRepository> recipeRepositoryDelegate,
        Func<CancellationToken, IRecipeFactory> recipeFactoryDelegate,
        CancellationToken cancellationToken)
    {
        _recipeRepository = recipeRepositoryDelegate(cancellationToken);
        _recipeFactory = recipeFactoryDelegate(cancellationToken);
    }

    public async Task<IRecipe> CreateAsync(RecipeCreation creation)
    {
        var recipe = await _recipeFactory.CreateNewAsync(creation);
        return await _recipeRepository.StoreAsync(recipe);
    }
}