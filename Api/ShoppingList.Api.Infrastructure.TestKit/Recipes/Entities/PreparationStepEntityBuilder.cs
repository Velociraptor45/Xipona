using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Recipes.Entities;

public class PreparationStepEntityBuilder : TestBuilderBase<PreparationStep>
{
    public PreparationStepEntityBuilder()
    {
        WithRecipe(null);
    }

    public PreparationStepEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public PreparationStepEntityBuilder WithRecipeId(Guid recipeId)
    {
        FillPropertyWith(p => p.RecipeId, recipeId);
        return this;
    }

    public PreparationStepEntityBuilder WithInstruction(string instruction)
    {
        FillPropertyWith(p => p.Instruction, instruction);
        return this;
    }

    public PreparationStepEntityBuilder WithSortingIndex(int sortingIndex)
    {
        FillPropertyWith(p => p.SortingIndex, sortingIndex);
        return this;
    }

    public PreparationStepEntityBuilder WithRecipe(Recipe? recipe)
    {
        FillPropertyWith(p => p.Recipe, recipe);
        return this;
    }

    public PreparationStepEntityBuilder WithoutRecipe()
    {
        return WithRecipe(null);
    }
}