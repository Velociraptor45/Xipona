using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingList
    {
        private IEnumerable<ShoppingListItem> items;

        public ShoppingList(ShoppingListId id, Store store, IEnumerable<ShoppingListItem> items, DateTime? completionDate)
        {
            var item = items.FirstOrDefault(i => !i.Id.IsActualId);
            if (item != null)
                throw new ActualIdRequiredException(item.Id);

            Id = id;
            Store = store;
            this.items = items;
            CompletionDate = completionDate;
        }

        public ShoppingListId Id { get; }
        public Store Store { get; }
        public IReadOnlyCollection<ShoppingListItem> Items => items.ToList().AsReadOnly();
        public DateTime? CompletionDate { get; private set; }

        public void AddItem(IStoreItem storeItem, bool isInBasket, float quantity)
        {
            if (storeItem == null)
                throw new ArgumentNullException(nameof(storeItem));
            if (!storeItem.Id.IsActualId)
                throw new ActualIdRequiredException(storeItem.Id);

            var list = items.ToList();

            var existingItem = list.FirstOrDefault(it => it.Id == storeItem.Id.ToShoppingListItemId());
            if (existingItem != null)
                throw new ItemAlreadyOnShoppingListException($"Item {storeItem.Id} already exists on shopping list {Id.Value}");

            IStoreItemAvailability availability = storeItem.Availabilities
                .FirstOrDefault(availability => availability.StoreId == Store.Id);
            if (availability == null)
                throw new ItemAtStoreNotAvailableException(storeItem.Id, Store.Id);

            list.Add(storeItem.ToShoppingListItemDomain(availability.Price, isInBasket, quantity));
            items = list;
        }

        public void RemoveItem(ShoppingListItemId id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (!id.IsActualId)
                throw new ActualIdRequiredException(id);

            var itemList = items.ToList();

            var itemListWithoutSpecifiedItem = itemList
                .Where(item => item.Id != id)
                .ToList();

            if (itemList.Count == itemListWithoutSpecifiedItem.Count)
                throw new ItemNotOnShoppingListException("Item is not on shopping list");

            items = itemListWithoutSpecifiedItem;
        }

        public void PutItemInBasket(ShoppingListItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));
            if (!itemId.IsActualId)
                throw new ActualIdRequiredException(itemId);

            var item = items.FirstOrDefault(item => item.Id == itemId);
            if (item == null)
                throw new ItemNotOnShoppingListException(Id, itemId);

            item.PutInBasket();

            var updatedList = items
                .Where(item => item.Id != itemId)
                .ToList();
            updatedList.Add(item);

            items = updatedList;
        }

        public void RemoveFromBasket(ShoppingListItemId itemId)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));
            if (!itemId.IsActualId)
                throw new ActualIdRequiredException(itemId);

            var item = items.FirstOrDefault(item => item.Id == itemId);
            if (item == null)
                throw new ItemNotOnShoppingListException(Id, itemId);

            item.RemoveFromBasket();

            var updatedList = items
                .Where(item => item.Id != itemId)
                .ToList();
            updatedList.Add(item);

            items = updatedList;
        }

        public void ChangeItemQuantity(ShoppingListItemId itemId, float quantity)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));
            if (!itemId.IsActualId)
                throw new ActualIdRequiredException(itemId);
            if (quantity <= 0f)
                throw new InvalidItemQuantityException(quantity);

            var item = items.FirstOrDefault(item => item.Id == itemId);
            if (item == null)
                throw new ItemNotOnShoppingListException(Id, itemId);

            item.ChangeQuantity(quantity);

            var updatedList = items
                .Where(item => item.Id != itemId)
                .ToList();
            updatedList.Add(item);

            items = updatedList;
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
            CompletionDate = completionDate;

            return new ShoppingList(new ShoppingListId(0), Store, itemsNotInBasket, null);
        }
    }
}