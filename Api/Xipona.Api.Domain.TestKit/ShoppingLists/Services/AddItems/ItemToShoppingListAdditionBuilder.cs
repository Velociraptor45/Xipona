using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.AddItems;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.TestKit.Common;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Services.AddItems;
public class ItemToShoppingListAdditionBuilder : DomainTestBuilderBase<ItemToShoppingListAddition>
{
    public ItemToShoppingListAdditionBuilder WithItemId(ItemId itemId)
    {
        FillConstructorWith(nameof(itemId), itemId);
        return this;
    }

    public ItemToShoppingListAdditionBuilder WithItemTypeId(ItemTypeId? itemTypeId)
    {
        FillConstructorWith(nameof(itemTypeId), itemTypeId);
        return this;
    }

    public ItemToShoppingListAdditionBuilder WithoutItemTypeId()
    {
        return WithItemTypeId(null);
    }

    public ItemToShoppingListAdditionBuilder WithStoreId(StoreId storeId)
    {
        FillConstructorWith(nameof(storeId), storeId);
        return this;
    }

    public ItemToShoppingListAdditionBuilder WithQuantity(QuantityInBasket quantity)
    {
        FillConstructorWith(nameof(quantity), quantity);
        return this;
    }
}