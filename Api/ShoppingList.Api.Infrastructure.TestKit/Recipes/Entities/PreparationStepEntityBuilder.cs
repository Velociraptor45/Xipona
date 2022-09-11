using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Recipes.Entities;

public class PreparationStepEntityBuilder : TestBuilder<PreparationStep>
{
    public PreparationStepEntityBuilder()
    {
        WithRecipe(null);
    }

    public PreparationStepEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(e => e.Id, id);
        return this;
    }

    public PreparationStepEntityBuilder WithRecipeId(Guid id)
    {
        FillPropertyWith(e => e.RecipeId, id);
        return this;
    }

    public PreparationStepEntityBuilder WithRecipe(Recipe? recipe)
    {
        FillPropertyWith(i => i.Recipe, recipe);
        return this;
    }
}