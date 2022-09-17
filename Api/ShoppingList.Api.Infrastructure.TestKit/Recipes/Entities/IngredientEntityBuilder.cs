using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using Ingredient = ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities.Ingredient;
using Recipe = ProjectHermes.ShoppingList.Api.Infrastructure.Recipes.Entities.Recipe;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Recipes.Entities;

public class IngredientEntityBuilder : TestBuilderBase<Ingredient>
{
    public IngredientEntityBuilder()
    {
        WithRecipe(null);
        WithQuantityType(new DomainTestBuilder<IngredientQuantityType>().Create().ToInt());
    }

    public IngredientEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public IngredientEntityBuilder WithRecipeId(Guid recipeId)
    {
        FillPropertyWith(p => p.RecipeId, recipeId);
        return this;
    }

    public IngredientEntityBuilder WithItemCategoryId(Guid itemCategoryId)
    {
        FillPropertyWith(p => p.ItemCategoryId, itemCategoryId);
        return this;
    }

    public IngredientEntityBuilder WithQuantityType(int quantityType)
    {
        FillPropertyWith(p => p.QuantityType, quantityType);
        return this;
    }

    public IngredientEntityBuilder WithQuantity(float quantity)
    {
        FillPropertyWith(p => p.Quantity, quantity);
        return this;
    }

    public IngredientEntityBuilder WithRecipe(Recipe? recipe)
    {
        FillPropertyWith(p => p.Recipe, recipe);
        return this;
    }

    public IngredientEntityBuilder WithoutRecipe()
    {
        return WithRecipe(null);
    }
}