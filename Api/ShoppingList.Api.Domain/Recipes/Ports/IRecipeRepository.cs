using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;

public interface IRecipeRepository
{
    Task<IRecipe> StoreAsync(IRecipe recipe);

    Task<IEnumerable<RecipeSearchResult>> SearchByAsync(string searchInput);

    Task<IRecipe?> FindByAsync(RecipeId recipeId);

    Task<IEnumerable<IRecipe>> FindByAsync(ItemId defaultItemId);

    Task<IEnumerable<IRecipe>> FindByAsync(ItemId defaultItemId, ItemTypeId? defaultItemTypeId, StoreId defaultStoreId);

    Task<IEnumerable<IRecipe>> FindByContainingAllAsync(IEnumerable<RecipeTagId> recipeTagIds);
}