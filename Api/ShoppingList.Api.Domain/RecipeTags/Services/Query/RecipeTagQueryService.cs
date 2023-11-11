using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Services.Query;

public class RecipeTagQueryService : IRecipeTagQueryService
{
    private readonly IRecipeTagRepository _recipeTagRepository;

    public RecipeTagQueryService(IRecipeTagRepository recipeTagRepository)
    {
        _recipeTagRepository = recipeTagRepository;
    }

    public async Task<IEnumerable<IRecipeTag>> GetAllAsync()
    {
        return await _recipeTagRepository.FindAllAsync();
    }
}