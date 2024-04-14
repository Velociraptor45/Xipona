using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.Xipona.Api.Domain.TestKit.ShoppingLists.Models;

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
        var items = new List<ShoppingListItem>
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

    public static ShoppingListSectionBuilder Items(IEnumerable<ShoppingListItem> items)
    {
        return new ShoppingListSectionBuilder()
            .WithItems(items);
    }

    public static ShoppingListSectionBuilder Item(ItemId itemId, ItemTypeId? itemTypeId)
    {
        return new ShoppingListSectionBuilder()
            .WithItem(new ShoppingListItemBuilder().WithId(itemId).WithTypeId(itemTypeId).Create());
    }
}