using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;

public interface IRecipeRepository
{
    Task<IRecipe> StoreAsync(IRecipe recipe);

    Task<IEnumerable<RecipeSearchResult>> SearchByAsync(string searchInput);
    Task<IRecipe?> FindByAsync(RecipeId recipeId);
}