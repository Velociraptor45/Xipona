using ProjectHermes.Xipona.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Models.Factories;

public interface IRecipeFactory
{
    Task<IRecipe> CreateNewAsync(RecipeCreation creation);

    IRecipe Create(RecipeId id, RecipeName name, NumberOfServings numberOfServings,
        IEnumerable<IIngredient> ingredients, IEnumerable<IPreparationStep> steps,
        IEnumerable<RecipeTagId> recipeTagIds, RecipeId? sideDishId, DateTimeOffset createdAt);
}