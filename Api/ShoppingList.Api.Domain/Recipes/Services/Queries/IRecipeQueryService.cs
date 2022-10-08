using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

public interface IRecipeQueryService
{
    Task<IRecipe> GetAsync(RecipeId id);

    Task<IEnumerable<RecipeSearchResult>> SearchByNameAsync(string searchInput);
}