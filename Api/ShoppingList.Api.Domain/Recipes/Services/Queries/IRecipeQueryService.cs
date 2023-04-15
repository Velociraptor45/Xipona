using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

public interface IRecipeQueryService
{
    Task<IRecipe> GetAsync(RecipeId id);

    Task<IEnumerable<RecipeSearchResult>> SearchByNameAsync(string searchInput);

    Task<IEnumerable<RecipeSearchResult>> SearchByTagIdsAsync(IEnumerable<RecipeTagId> tagIds);
}