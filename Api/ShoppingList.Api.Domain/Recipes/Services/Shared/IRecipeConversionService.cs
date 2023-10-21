using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Shared;

public interface IRecipeConversionService
{
    Task<RecipeReadModel> ToReadModelAsync(IRecipe recipe);
}