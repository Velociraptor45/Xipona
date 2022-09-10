using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Recipes.Entities;

public class RecipeEntityBuilder : TestBuilder<Recipe>
{
    public RecipeEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(e => e.Id, id);
        return this;
    }

    public RecipeEntityBuilder WithIngredients(IEnumerable<Ingredient> ingredients)
    {
        FillPropertyWith(e => e.Ingredients, ingredients.ToList());
        return this;
    }

    public RecipeEntityBuilder WithPreparationSteps(IEnumerable<PreparationStep> preparationSteps)
    {
        FillPropertyWith(e => e.PreparationSteps, preparationSteps.ToList());
        return this;
    }
}