using ProjectHermes.Xipona.Api.Core.Extensions;
using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;
using Ingredient = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Ingredient;
using Recipe = ProjectHermes.Xipona.Api.Repositories.Recipes.Entities.Recipe;

namespace ProjectHermes.Xipona.Api.Repositories.TestKit.Recipes.Entities;

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

    public IngredientEntityBuilder WithDefaultStoreId(Guid? defaultStoreId)
    {
        FillPropertyWith(p => p.DefaultStoreId, defaultStoreId);
        return this;
    }

    public IngredientEntityBuilder WithoutDefaultStoreId()
    {
        return WithDefaultStoreId(null);
    }

    public IngredientEntityBuilder WithAddToShoppingListByDefault(bool? addToShoppingListByDefault)
    {
        FillPropertyWith(p => p.AddToShoppingListByDefault, addToShoppingListByDefault);
        return this;
    }

    public IngredientEntityBuilder WithoutAddToShoppingListByDefault()
    {
        return WithAddToShoppingListByDefault(null);
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