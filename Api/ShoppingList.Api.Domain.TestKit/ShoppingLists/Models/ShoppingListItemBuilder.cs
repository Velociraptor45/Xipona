﻿using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

public class ShoppingListItemBuilder : DomainTestBuilderBase<ShoppingListItem>
{
    public ShoppingListItemBuilder()
    {
        Customize(new QuantityInBasketCustomization());
    }

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

    public ShoppingListItemBuilder WithQuantity(QuantityInBasket quantity)
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