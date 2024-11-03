using ProjectHermes.Xipona.Api.Core.TestKit;
using ProjectHermes.Xipona.Api.Repositories.ShoppingLists.Entities;
using System;

namespace ProjectHermes.Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;

public class DiscountEntityBuilder : TestBuilderBase<Discount>
{
    public DiscountEntityBuilder()
    {
        WithoutShoppingList();
    }

    public DiscountEntityBuilder WithId(Guid id)
    {
        FillPropertyWith(p => p.Id, id);
        return this;
    }

    public DiscountEntityBuilder WithDiscountPrice(decimal discountPrice)
    {
        FillPropertyWith(p => p.DiscountPrice, discountPrice);
        return this;
    }

    public DiscountEntityBuilder WithItemId(Guid itemId)
    {
        FillPropertyWith(p => p.ItemId, itemId);
        return this;
    }

    public DiscountEntityBuilder WithItemTypeId(Guid? itemTypeId)
    {
        FillPropertyWith(p => p.ItemTypeId, itemTypeId);
        return this;
    }

    public DiscountEntityBuilder WithoutItemTypeId()
    {
        return WithItemTypeId(null);
    }

    public DiscountEntityBuilder WithShoppingListId(Guid shoppingListId)
    {
        FillPropertyWith(p => p.ShoppingListId, shoppingListId);
        return this;
    }

    public DiscountEntityBuilder WithShoppingList(ShoppingList? shoppingList)
    {
        FillPropertyWith(p => p.ShoppingList, shoppingList);
        return this;
    }

    public DiscountEntityBuilder WithoutShoppingList()
    {
        return WithShoppingList(null);
    }
}