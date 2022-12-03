using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities;

namespace ProjectHermes.ShoppingList.Api.Repositories.TestKit.Recipes.Entities;

public class RecipeEntityBuilder : TestBuilderBase<Recipe>
{
    public RecipeEntityBuilder()
    {
        WithIngredients(new IngredientEntityBuilder().CreateMany(3).ToArray());
        WithPreparationSteps(new PreparationStepEntityBuilder().CreateMany(3).ToArray());
    }

    public RecipeEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public RecipeEntityBuilder WithName(string name)
    {
        FillPropertyWith(p => p.Name, name);
        return this;
    }

    public RecipeEntityBuilder WithIngredients(ICollection<Ingredient> ingredients)
    {
        FillPropertyWith(p => p.Ingredients, ingredients);
        return this;
    }

    public RecipeEntityBuilder WithPreparationSteps(ICollection<PreparationStep> preparationSteps)
    {
        FillPropertyWith(p => p.PreparationSteps, preparationSteps);
        return this;
    }
}