using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Creation;

public class RecipeTagCreationService : IRecipeTagCreationService
{
    private readonly IRecipeTagFactory _recipeTagFactory;
    private readonly IRecipeTagRepository _recipeTagRepository;

    public RecipeTagCreationService(
        IRecipeTagFactory recipeTagFactory,
        Func<CancellationToken, IRecipeTagRepository> recipeTagRepositoryDelegate,
        CancellationToken cancellationToken)
    {
        _recipeTagFactory = recipeTagFactory;
        _recipeTagRepository = recipeTagRepositoryDelegate(cancellationToken);
    }

    public async Task<IRecipeTag> CreateAsync(string name)
    {
        var recipeTag = _recipeTagFactory.CreateNew(name);
        recipeTag = await _recipeTagRepository.StoreAsync(recipeTag);
        return recipeTag;
    }
}