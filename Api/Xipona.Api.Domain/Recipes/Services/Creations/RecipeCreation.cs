using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Creations;

public class RecipeCreation
{
    private readonly List<IngredientCreation> _ingredientCreations;
    private readonly List<PreparationStepCreation> _preparationStepCreations;

    public RecipeCreation(RecipeName name, NumberOfServings numberOfServings,
        IEnumerable<IngredientCreation> ingredientCreations,
        IEnumerable<PreparationStepCreation> preparationStepCreations, RecipeId? sideDishId,
        IEnumerable<RecipeTagId> recipeTagIds)
    {
        _ingredientCreations = ingredientCreations.ToList();
        _preparationStepCreations = preparationStepCreations.ToList();
        Name = name;
        NumberOfServings = numberOfServings;
        SideDishId = sideDishId;
        RecipeTagIds = recipeTagIds.ToList();
    }

    public RecipeName Name { get; }
    public NumberOfServings NumberOfServings { get; }
    public RecipeId? SideDishId { get; }
    public IReadOnlyCollection<IngredientCreation> IngredientCreations => _ingredientCreations;
    public IReadOnlyCollection<PreparationStepCreation> PreparationStepCreations => _preparationStepCreations;
    public IReadOnlyCollection<RecipeTagId> RecipeTagIds { get; }
}