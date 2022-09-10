using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using Ingredient = ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities.Ingredient;
using Recipe = ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities.Recipe;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Recipes.Entities;

public class IngredientEntityBuilder : TestBuilder<Ingredient>
{
    public IngredientEntityBuilder()
    {
        WithRecipe(null);
        WithQuantityType(new DomainTestBuilder<IngredientQuantityType>().Create().ToInt());
    }

    public IngredientEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(e => e.Id, id);
        return this;
    }

    public IngredientEntityBuilder WithRecipeId(Guid id)
    {
        FillPropertyWith(e => e.RecipeId, id);
        return this;
    }

    public IngredientEntityBuilder WithQuantityType(int quantityType)
    {
        FillPropertyWith(e => e.QuantityType, quantityType);
        return this;
    }

    public IngredientEntityBuilder WithRecipe(Recipe? recipe)
    {
        FillPropertyWith(e => e.Recipe, recipe);
        return this;
    }
}