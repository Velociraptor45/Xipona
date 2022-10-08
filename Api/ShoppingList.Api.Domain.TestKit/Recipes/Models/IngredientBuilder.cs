using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.Recipes.Models;

public class IngredientBuilder : DomainTestBuilderBase<Ingredient>
{
    public IngredientBuilder WithId(IngredientId id)
    {
        FillConstructorWith(nameof(id), id);
        return this;
    }

    public IngredientBuilder WithItemCategoryId(ItemCategoryId itemCategoryId)
    {
        FillConstructorWith(nameof(itemCategoryId), itemCategoryId);
        return this;
    }

    public IngredientBuilder WithQuantityType(IngredientQuantityType quantityType)
    {
        FillConstructorWith(nameof(quantityType), quantityType);
        return this;
    }

    public IngredientBuilder WithQuantity(IngredientQuantity quantity)
    {
        FillConstructorWith(nameof(quantity), quantity);
        return this;
    }

    public IngredientBuilder WithDefaultItemId(ItemId? defaultItemId)
    {
        FillConstructorWith(nameof(defaultItemId), defaultItemId);
        return this;
    }

    public IngredientBuilder WithoutDefaultItemId()
    {
        return WithDefaultItemId(null);
    }

    public IngredientBuilder WithDefaultItemTypeId(ItemTypeId? defaultItemTypeId)
    {
        FillConstructorWith(nameof(defaultItemTypeId), defaultItemTypeId);
        return this;
    }

    public IngredientBuilder WithoutDefaultItemTypeId()
    {
        return WithDefaultItemTypeId(null);
    }
}