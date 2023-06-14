using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;
public class IngredientShoppingListPropertiesBuilder : DomainTestBuilderBase<IngredientShoppingListProperties>
{
    public IngredientShoppingListPropertiesBuilder WithDefaultItemId(ItemId defaultItemId)
    {
        FillConstructorWith(nameof(defaultItemId), defaultItemId);
        return this;
    }

    public IngredientShoppingListPropertiesBuilder WithDefaultItemTypeId(ItemTypeId? defaultItemTypeId)
    {
        FillConstructorWith(nameof(defaultItemTypeId), defaultItemTypeId);
        return this;
    }

    public IngredientShoppingListPropertiesBuilder WithoutDefaultItemTypeId()
    {
        return WithDefaultItemTypeId(null);
    }

    public IngredientShoppingListPropertiesBuilder WithDefaultStoreId(StoreId defaultStoreId)
    {
        FillConstructorWith(nameof(defaultStoreId), defaultStoreId);
        return this;
    }

    public IngredientShoppingListPropertiesBuilder WithAddToShoppingListByDefault(bool addToShoppingListByDefault)
    {
        FillConstructorWith(nameof(addToShoppingListByDefault), addToShoppingListByDefault);
        return this;
    }
}