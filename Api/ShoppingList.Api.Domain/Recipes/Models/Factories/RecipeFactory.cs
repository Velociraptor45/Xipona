using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models.Factories;

public class RecipeFactory : IRecipeFactory
{
    private readonly IIngredientFactory _ingredientFactory;
    private readonly IPreparationStepFactory _preparationStepFactory;
    private readonly IValidator _validator;

    public RecipeFactory(IIngredientFactory ingredientFactory, IValidator validator,
        IPreparationStepFactory preparationStepFactory)
    {
        _validator = validator;
        _ingredientFactory = ingredientFactory;
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

        await _validator.ValidateAsync(creation.RecipeTagIds);

        return new Recipe(
            RecipeId.New,
            creation.Name,
            creation.NumberOfServings,
            new Ingredients(ingredients, _ingredientFactory),
            new PreparationSteps(preparationSteps, _preparationStepFactory),
            new RecipeTags(creation.RecipeTagIds));
    }

    public IRecipe Create(RecipeId id, RecipeName name, NumberOfServings numberOfServings, IEnumerable<IIngredient> ingredients,
        IEnumerable<IPreparationStep> steps, IEnumerable<RecipeTagId> recipeTagIds)
    {
        return new Recipe(
            id,
            name,
            numberOfServings,
            new Ingredients(ingredients, _ingredientFactory),
            new PreparationSteps(steps, _preparationStepFactory),
            new RecipeTags(recipeTagIds));
    }
}