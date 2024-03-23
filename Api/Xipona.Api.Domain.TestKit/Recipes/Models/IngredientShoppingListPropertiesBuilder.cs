using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Recipes.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.Recipes.Models;

public class IngredientShoppingListPropertiesBuilder : DomainRecordTestBuilderBase<IngredientShoppingListProperties>
{
    public IngredientShoppingListPropertiesBuilder()
    {
    }

    public IngredientShoppingListPropertiesBuilder(IngredientShoppingListProperties properties)
    {
        WithDefaultItemId(properties.DefaultItemId);
        WithDefaultItemTypeId(properties.DefaultItemTypeId);
        WithDefaultStoreId(properties.DefaultStoreId);
        WithAddToShoppingListByDefault(properties.AddToShoppingListByDefault);
    }

    public IngredientShoppingListPropertiesBuilder WithDefaultItemId(ItemId defaultItemId)
    {
        Modifiers.Add(x => x with { DefaultItemId = defaultItemId });
        return this;
    }

    public IngredientShoppingListPropertiesBuilder WithDefaultItemTypeId(ItemTypeId? defaultItemTypeId)
    {
        Modifiers.Add(x => x with { DefaultItemTypeId = defaultItemTypeId });
        return this;
    }

    public IngredientShoppingListPropertiesBuilder WithoutDefaultItemTypeId()
    {
        return WithDefaultItemTypeId(null);
    }

    public IngredientShoppingListPropertiesBuilder WithDefaultStoreId(StoreId defaultStoreId)
    {
        Modifiers.Add(x => x with { DefaultStoreId = defaultStoreId });
        return this;
    }

    public IngredientShoppingListPropertiesBuilder WithAddToShoppingListByDefault(bool addToShoppingListByDefault)
    {
        Modifiers.Add(x => x with { AddToShoppingListByDefault = addToShoppingListByDefault });
        return this;
    }
}