using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Adapters;

public class RecipeRepository : IRecipeRepository
{
    public Task<IRecipe> StoreAsync(IRecipe recipe)
    {
        throw new NotImplementedException();
    }
}