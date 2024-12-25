using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Modifications;

public class RecipeModification
{
    public RecipeModification(RecipeId id, RecipeName name, NumberOfServings numberOfServings,
        IEnumerable<IngredientModification> ingredientModifications,
        IEnumerable<PreparationStepModification> preparationStepModifications, IEnumerable<RecipeTagId> recipeTagIds,
        RecipeId? sideDishId)
    {
        Id = id;
        Name = name;
        NumberOfServings = numberOfServings;
        SideDishId = sideDishId;
        RecipeTagIds = recipeTagIds.ToList();
        IngredientModifications = ingredientModifications.ToList();
        PreparationStepModifications = preparationStepModifications.ToList();
    }

    public RecipeId Id { get; }
    public RecipeName Name { get; }
    public NumberOfServings NumberOfServings { get; }
    public RecipeId? SideDishId { get; }
    public IReadOnlyCollection<IngredientModification> IngredientModifications { get; }
    public IReadOnlyCollection<PreparationStepModification> PreparationStepModifications { get; }
    public IReadOnlyCollection<RecipeTagId> RecipeTagIds { get; }
}