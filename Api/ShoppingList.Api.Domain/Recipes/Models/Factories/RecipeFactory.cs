using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;

public class RecipeFactory : IRecipeFactory
{
    private readonly IIngredientFactory _ingredientFactory;
    private readonly IPreparationStepFactory _preparationStepFactory;

    public RecipeFactory(
        Func<CancellationToken, IIngredientFactory> ingredientFactoryDelegate,
        IPreparationStepFactory preparationStepFactory,
        CancellationToken cancellationToken)
    {
        _ingredientFactory = ingredientFactoryDelegate(cancellationToken);
        _preparationStepFactory = preparationStepFactory;
    }

    public async Task<IRecipe> CreateNewAsync(RecipeCreation creation)
    {
        var ingredients = new List<IIngredient>();

        foreach (var ingredientCreation in creation.IngredientCreations)
        {
            var ingredient = await _ingredientFactory.CreateNewAsync(ingredientCreation);
            ingredients.Add(ingredient);
        }

        var preparationSteps = creation.PreparationStepCreations.Select(_preparationStepFactory.CreateNew);

        return new Recipe(RecipeId.New, creation.Name, new Ingredients(ingredients),
            new PreparationSteps(preparationSteps));
    }

    public IRecipe Create(RecipeId id, RecipeName name, IEnumerable<IIngredient> ingredients,
        IEnumerable<IPreparationStep> steps)
    {
        return new Recipe(
            id,
            name,
            new Ingredients(ingredients),
            new PreparationSteps(steps));
    }
}