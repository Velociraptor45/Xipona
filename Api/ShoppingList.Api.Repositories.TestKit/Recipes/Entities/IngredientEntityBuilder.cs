using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using Ingredient = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Ingredient;
using Recipe = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Recipe;

namespace ProjectHermes.ShoppingList.Api.Repositories.TestKit.Recipes.Entities;

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

    public IngredientEntityBuilder WithDefaultItemId(Guid? defaultItemId)
    {
        FillPropertyWith(p => p.DefaultItemId, defaultItemId);
        return this;
    }

    public IngredientEntityBuilder WithoutDefaultItemId()
    {
        return WithDefaultItemId(null);
    }

    public IngredientEntityBuilder WithDefaultItemTypeId(Guid? defaultItemTypeId)
    {
        FillPropertyWith(p => p.DefaultItemTypeId, defaultItemTypeId);
        return this;
    }

    public IngredientEntityBuilder WithoutDefaultItemTypeId()
    {
        return WithDefaultItemTypeId(null);
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