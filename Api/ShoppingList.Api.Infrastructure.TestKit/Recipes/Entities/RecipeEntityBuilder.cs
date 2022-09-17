using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Recipes.Entities;

public class RecipeEntityBuilder : TestBuilderBase<Recipe>
{
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