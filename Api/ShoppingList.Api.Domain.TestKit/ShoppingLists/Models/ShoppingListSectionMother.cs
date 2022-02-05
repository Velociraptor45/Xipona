using System.Collections.Generic;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models;

public static class ShoppingListSectionMother
{
    public static ShoppingListSectionBuilder OneItemInBasket()
    {
        var item = ShoppingListItemMother.InBasket().WithoutTypeId().Create();

        return new ShoppingListSectionBuilder()
            .WithItem(item);
    }

    public static ShoppingListSectionBuilder OneItemNotInBasket()
    {
        var item = ShoppingListItemMother.NotInBasket().WithoutTypeId().Create();

        return new ShoppingListSectionBuilder()
            .WithItem(item);
    }

    public static ShoppingListSectionBuilder OneItemInBasketAndOneNot()
    {
        var items = new List<IShoppingListItem>
        {
            ShoppingListItemMother.NotInBasket().WithoutTypeId().Create(),
            ShoppingListItemMother.InBasket().WithoutTypeId().Create()
        };

        return new ShoppingListSectionBuilder()
            .WithItems(items);
    }

    public static ShoppingListSectionBuilder Empty()
    {
        return new ShoppingListSectionBuilder()
            .WithoutItems();
    }

    public static ShoppingListSectionBuilder Items(IEnumerable<IShoppingListItem> items)
    {
        return new ShoppingListSectionBuilder()
            .WithItems(items);
    }
}