using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;

public interface IRecipeFactory
{
    Task<IRecipe> CreateNewAsync(RecipeCreation creation);
}