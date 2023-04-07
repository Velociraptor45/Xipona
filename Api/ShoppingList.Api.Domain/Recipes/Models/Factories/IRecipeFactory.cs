using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;

public interface IRecipeFactory
{
    Task<IRecipe> CreateNewAsync(RecipeCreation creation);

    IRecipe Create(RecipeId id, RecipeName name, IEnumerable<IIngredient> ingredients,
        IEnumerable<IPreparationStep> steps, IEnumerable<RecipeTagId> recipeTagIds);
}