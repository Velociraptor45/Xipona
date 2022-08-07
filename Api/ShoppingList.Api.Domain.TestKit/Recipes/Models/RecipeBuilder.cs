using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;

public class RecipeBuilder : DomainTestBuilderBase<Recipe>
{
    public RecipeBuilder WithId(RecipeId id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public RecipeBuilder WithName(RecipeName name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }

    public RecipeBuilder WithIngredients(Ingredients ingredients)
    {
        FillConstructorWith(nameof(ingredients), ingredients);
        return this;
    }

    public RecipeBuilder WithSteps(PreparationSteps steps)
    {
        FillConstructorWith(nameof(steps), steps);
        return this;
    }
}