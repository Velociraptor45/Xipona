namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class Recipe : IRecipe
{
    private readonly Ingredients _ingredients;
    private readonly PreparationSteps _steps;

    public Recipe(RecipeId id, RecipeName name, Ingredients ingredients, PreparationSteps steps)
    {
        _ingredients = ingredients;
        _steps = steps;
        Id = id;
        Name = name;
    }

    public RecipeId Id { get; }
    public RecipeName Name { get; }
    public IReadOnlyCollection<IIngredient> Ingredients => _ingredients.AsReadOnly();
    public IReadOnlyCollection<IPreparationStep> PreparationSteps => _steps.AsReadOnly();
}