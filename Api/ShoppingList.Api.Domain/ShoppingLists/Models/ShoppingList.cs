using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingList : IShoppingList
    {
        private IEnumerable<IShoppingListItem> items;

        public ShoppingList(ShoppingListId id, IStore store, IEnumerable<IShoppingListItem> items, DateTime? completionDate)
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
        public IStore Store { get; }
        public IReadOnlyCollection<IShoppingListItem> Items => items.ToList().AsReadOnly();
        public DateTime? CompletionDate { get; private set; }

        public void AddItem(IShoppingListItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            if (!item.Id.IsActualId)
                throw new ActualIdRequiredException(item.Id);

            var list = items.ToList();

            var existingItem = list.FirstOrDefault(it => it.Id == item.Id);
            if (existingItem != null)
                throw new ItemAlreadyOnShoppingListException($"Item {item.Id} already exists on shopping list {Id.Value}");

            list.Add(item);
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
        public IShoppingList Finish(DateTime completionDate)
        {
            var itemsNotInBasket = items.Where(i => !i.IsInBasket);

            items = items.Where(i => i.IsInBasket);
            CompletionDate = completionDate;

            return new ShoppingList(new ShoppingListId(0), Store, itemsNotInBasket, null);
        }
    }
}