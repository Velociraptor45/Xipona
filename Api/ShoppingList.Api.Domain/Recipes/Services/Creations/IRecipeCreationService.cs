using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

public interface IRecipeCreationService
{
    Task<IRecipe> CreateAsync(RecipeCreation creation);
}