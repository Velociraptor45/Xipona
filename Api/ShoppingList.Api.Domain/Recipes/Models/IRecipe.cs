namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public interface IRecipe
{
    RecipeId Id { get; }
    RecipeName Name { get; }
    IReadOnlyCollection<IIngredient> Ingredients { get; }
    IReadOnlyCollection<IPreparationStep> PreparationSteps { get; }
}