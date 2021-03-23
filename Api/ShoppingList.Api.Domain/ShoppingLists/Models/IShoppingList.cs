using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
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

        void AddItem(IShoppingListItem item, SectionId sectionId);

        void RemoveItem(ItemId itemId);

        void PutItemInBasket(ItemId itemId);

        void RemoveFromBasket(ItemId itemId);

        void ChangeItemQuantity(ItemId itemId, float quantity);

        IShoppingList Finish(DateTime completionDate);
    }
}