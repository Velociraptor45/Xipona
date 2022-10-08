using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;

public class RecipeModification
{
    public RecipeModification(RecipeId id, RecipeName name, IEnumerable<IngredientModification> ingredientModifications,
        IEnumerable<PreparationStepModification> preparationStepModifications)
    {
        Id = id;
        Name = name;
        IngredientModifications = ingredientModifications.ToList();
        PreparationStepModifications = preparationStepModifications.ToList();
    }

    public RecipeId Id { get; }
    public RecipeName Name { get; }
    public IReadOnlyCollection<IngredientModification> IngredientModifications { get; }
    public IReadOnlyCollection<PreparationStepModification> PreparationStepModifications { get; }
}