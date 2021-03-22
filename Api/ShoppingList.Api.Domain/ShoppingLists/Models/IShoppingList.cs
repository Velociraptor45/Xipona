using ProjectHermes.ShoppingList.Api.Domain.Stores.Model;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public interface IShoppingList
    {
        public ShoppingListId Id { get; }
        public StoreId StoreId { get; }
        public DateTime? CompletionDate { get; }
        IReadOnlyCollection<IShoppingListSection> Sections { get; }
        public IReadOnlyCollection<IShoppingListItem> Items { get; }

        IShoppingList AddItem(IShoppingListItem item, ShoppingListSectionId sectionId);

        IShoppingList RemoveItem(ShoppingListItemId itemId);

        IShoppingList PutItemInBasket(ShoppingListItemId itemId);

        IShoppingList RemoveFromBasket(ShoppingListItemId itemId);

        IShoppingList ChangeItemQuantity(ShoppingListItemId itemId, float quantity);

        IShoppingList SetCompletionDate(DateTime completionDate);

        IShoppingList RemoveItemsInBasket();

        IShoppingList RemoveItemsNotInBasket();
    }
}