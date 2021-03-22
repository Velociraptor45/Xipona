using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
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

        IShoppingList AddItem(IShoppingListItem item, SectionId sectionId);

        IShoppingList RemoveItem(ItemId itemId);

        IShoppingList PutItemInBasket(ItemId itemId);

        IShoppingList RemoveFromBasket(ItemId itemId);

        IShoppingList ChangeItemQuantity(ItemId itemId, float quantity);

        IShoppingList SetCompletionDate(DateTime completionDate);

        IShoppingList RemoveItemsInBasket();

        IShoppingList RemoveItemsNotInBasket();
    }
}