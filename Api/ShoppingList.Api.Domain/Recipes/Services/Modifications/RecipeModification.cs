using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

public class RecipeModification
{
    public RecipeModification(RecipeId id, RecipeName name, NumberOfServings numberOfServings,
        IEnumerable<IngredientModification> ingredientModifications,
        IEnumerable<PreparationStepModification> preparationStepModifications, IEnumerable<RecipeTagId> recipeTagIds)
    {
        Id = id;
        Name = name;
        NumberOfServings = numberOfServings;
        RecipeTagIds = recipeTagIds.ToList();
        IngredientModifications = ingredientModifications.ToList();
        PreparationStepModifications = preparationStepModifications.ToList();
    }

    public RecipeId Id { get; }
    public RecipeName Name { get; }
    public NumberOfServings NumberOfServings { get; }
    public IReadOnlyCollection<IngredientModification> IngredientModifications { get; }
    public IReadOnlyCollection<PreparationStepModification> PreparationStepModifications { get; }
    public IReadOnlyCollection<RecipeTagId> RecipeTagIds { get; }
}