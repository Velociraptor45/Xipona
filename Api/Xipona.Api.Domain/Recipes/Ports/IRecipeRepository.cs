using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Ports;

public interface IRecipeRepository
{
    Task<IRecipe> StoreAsync(IRecipe recipe);

    Task<IEnumerable<RecipeSearchResult>> SearchByAsync(string searchInput);

    Task<IRecipe?> FindByAsync(RecipeId recipeId);

    Task<IEnumerable<IRecipe>> FindByAsync(ItemId defaultItemId);

    Task<IEnumerable<IRecipe>> FindByAsync(ItemId defaultItemId, ItemTypeId? defaultItemTypeId);

    Task<IEnumerable<IRecipe>> FindByAsync(ItemId defaultItemId, ItemTypeId? defaultItemTypeId, StoreId defaultStoreId);

    Task<IEnumerable<IRecipe>> FindByAsync(ItemCategoryId itemCategoryId);

    Task<IEnumerable<IRecipe>> FindByContainingAllAsync(IEnumerable<RecipeTagId> recipeTagIds);
}