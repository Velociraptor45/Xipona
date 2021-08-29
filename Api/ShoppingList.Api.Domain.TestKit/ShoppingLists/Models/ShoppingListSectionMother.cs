using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models
{
    public static class ShoppingListSectionMother
    {
        public static ShoppingListSectionBuilder OneItemInBasket()
        {
            var item = ShoppingListItemMother.InBasket().Create();

            return new ShoppingListSectionBuilder()
                .WithItem(item);
        }

        public static ShoppingListSectionBuilder OneItemNotInBasket()
        {
            var item = ShoppingListItemMother.NotInBasket().Create();

            return new ShoppingListSectionBuilder()
                .WithItem(item);
        }

        public static ShoppingListSectionBuilder OneItemInBasketAndOneNot()
        {
            var items = new List<IShoppingListItem>
            {
                ShoppingListItemMother.NotInBasket().Create(),
                ShoppingListItemMother.InBasket().Create()
            };

            return new ShoppingListSectionBuilder()
                .WithItems(items);
        }

        public static ShoppingListSectionBuilder Empty()
        {
            return new ShoppingListSectionBuilder()
                .WithoutItems();
        }
    }
}