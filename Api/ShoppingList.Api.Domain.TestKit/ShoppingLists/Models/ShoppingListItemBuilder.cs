using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Common;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

public class ShoppingListItemBuilder : DomainTestBuilderBase<ShoppingListItem>
{
    public ShoppingListItemBuilder WithId(ItemId id)
    {
        FillConstructorWith("id", id);
        return this;
    }

    public ShoppingListItemBuilder WithIsInBasket(bool isInBasket)
    {
        FillConstructorWith("isInBasket", isInBasket);
        return this;
    }

    public ShoppingListItemBuilder WithQuantity(float quantity)
    {
        FillConstructorWith("quantity", quantity);
        return this;
    }

    public ShoppingListItemBuilder WithTypeId(ItemTypeId? typeId)
    {
        FillConstructorWith("typeId", typeId);
        return this;
    }

    public ShoppingListItemBuilder WithoutTypeId()
    {
        return WithTypeId(null);
    }
}