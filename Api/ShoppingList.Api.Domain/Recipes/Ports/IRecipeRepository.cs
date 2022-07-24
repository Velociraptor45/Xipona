using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;

public interface IRecipeRepository
{
    Task<IRecipe> StoreAsync(IRecipe recipe);
}