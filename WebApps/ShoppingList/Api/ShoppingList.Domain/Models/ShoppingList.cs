using ShoppingList.Domain.Converters;
using ShoppingList.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Domain.Models
{
    public class ShoppingList
    {
        private IEnumerable<ShoppingListItem> items;

        public ShoppingList(ShoppingListId id, Store store, IEnumerable<ShoppingListItem> items, DateTime? completionDate)
        {
            Id = id;
            Store = store;
            this.items = items;
            CompletionDate = completionDate;
        }

        public ShoppingListId Id { get; }
        public Store Store { get; }
        public IReadOnlyCollection<ShoppingListItem> Items { get => items.ToList().AsReadOnly(); }
        public DateTime? CompletionDate { get; }

        public void AddItem(StoreItem storeItem, bool isInBasket, float quantity)
        {
            if (storeItem == null)
                throw new ArgumentNullException(nameof(storeItem));

            var list = items.ToList();

            var existingItem = list.FirstOrDefault(it => it.Id == storeItem.Id);
            if (existingItem != null)
                throw new ItemAlreadyOnShoppingListException($"Item {storeItem.Id.Value} already exists on shopping list {Id.Value}");

            list.Add(storeItem.ToShoppingListItemDomain(Id, isInBasket, quantity));
            items = list;
        }

        public void RemoveItem(ShoppingListItemId id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            var itemList = items.ToList();

            var itemListWithoutSpecifiedItem = itemList
                .Where(item => item.Id != id)
                .ToList();

            if (itemList.Count == itemListWithoutSpecifiedItem.Count)
                throw new InvalidOperationException("Item is not on shopping list");

            items = itemListWithoutSpecifiedItem;
        }
    }
}