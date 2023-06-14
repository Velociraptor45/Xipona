using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

public class RecipeCreation
{
    private readonly List<IngredientCreation> _ingredientCreations;
    private readonly List<PreparationStepCreation> _preparationStepCreations;

    public RecipeCreation(RecipeName name, NumberOfServings numberOfServings,
        IEnumerable<IngredientCreation> ingredientCreations,
        IEnumerable<PreparationStepCreation> preparationStepCreations, IEnumerable<RecipeTagId> recipeTagIds)
    {
        _ingredientCreations = ingredientCreations.ToList();
        _preparationStepCreations = preparationStepCreations.ToList();
        Name = name;
        NumberOfServings = numberOfServings;
        RecipeTagIds = recipeTagIds.ToList();
    }

    public RecipeName Name { get; }
    public NumberOfServings NumberOfServings { get; }
    public IReadOnlyCollection<IngredientCreation> IngredientCreations => _ingredientCreations;
    public IReadOnlyCollection<PreparationStepCreation> PreparationStepCreations => _preparationStepCreations;
    public IReadOnlyCollection<RecipeTagId> RecipeTagIds { get; }
}