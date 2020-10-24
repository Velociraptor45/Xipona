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
        private DateTime? completionDate;

        public ShoppingList(ShoppingListId id, Store store, IEnumerable<ShoppingListItem> items, DateTime? completionDate)
        {
            Id = id;
            Store = store;
            this.items = items;
            this.completionDate = completionDate;
        }

        public ShoppingListId Id { get; }
        public Store Store { get; }
        public IReadOnlyCollection<ShoppingListItem> Items { get => items.ToList().AsReadOnly(); }
        public DateTime? CompletionDate => completionDate;

        public void AddItem(StoreItem storeItem, ItemCategory itemCategory, Manufacturer manufacturer,
            bool isInBasket, float quantity)
        {
            if (storeItem == null)
                throw new ArgumentNullException(nameof(storeItem));

            var list = items.ToList();

            var existingItem = list.FirstOrDefault(it => it.Id == storeItem.Id);
            if (existingItem != null)
                throw new ItemAlreadyOnShoppingListException($"Item {storeItem.Id.Value} already exists on shopping list {Id.Value}");

            list.Add(storeItem.ToShoppingListItemDomain(Store.Id, itemCategory, manufacturer, isInBasket, quantity));
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
                throw new ItemNotOnShoppingListException("Item is not on shopping list");

            items = itemListWithoutSpecifiedItem;
        }

        /// <summary>
        /// Finishes the current shopping list and returns a new shopping list with all items that were not in the
        /// basket on it
        /// </summary>
        /// <returns></returns>
        public ShoppingList Finish(DateTime completionDate)
        {
            var itemsNotInBasket = items.Where(i => !i.IsInBasket);

            items = items.Where(i => i.IsInBasket);
            this.completionDate = completionDate;

            return new ShoppingList(new ShoppingListId(0), this.Store, itemsNotInBasket, null);
        }
    }
}