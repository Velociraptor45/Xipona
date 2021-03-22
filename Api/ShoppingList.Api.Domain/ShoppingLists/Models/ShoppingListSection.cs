using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingListSection : IShoppingListSection
    {
        private readonly Dictionary<ItemId, IShoppingListItem> shoppingListItems;

        public ShoppingListSection(ShoppingListSectionId id, IEnumerable<IShoppingListItem> shoppingListItems)
        {
            Id = id;
            this.shoppingListItems = shoppingListItems.ToDictionary(i => i.Id);
        }

        public ShoppingListSectionId Id { get; }

        public IReadOnlyCollection<IShoppingListItem> Items => shoppingListItems.Values.ToList().AsReadOnly();

        public IShoppingListSection RemoveItem(ItemId itemId)
        {
            if (!shoppingListItems.ContainsKey(itemId))
                return this;

            var items = new Dictionary<ItemId, IShoppingListItem>(shoppingListItems);
            items.Remove(itemId);

            return new ShoppingListSection(Id, items.Values);
        }

        public bool ContainsItem(ItemId itemId)
        {
            return shoppingListItems.ContainsKey(itemId);
        }

        public IShoppingListSection AddItem(IShoppingListItem item)
        {
            var items = new Dictionary<ItemId, IShoppingListItem>(shoppingListItems);

            if (items.ContainsKey(item.Id))
                throw new DomainException(new ItemAlreadyInSectionReason(item.Id, Id));

            items.Add(item.Id, item);
            return new ShoppingListSection(Id, items.Values);
        }

        public IShoppingListSection PutItemInBasket(ItemId itemId)
        {
            if (!shoppingListItems.ContainsKey(itemId))
                throw new DomainException(new ItemNotInSectionReason(itemId, Id));

            var items = new Dictionary<ItemId, IShoppingListItem>(shoppingListItems);
            var item = items[itemId];
            items[itemId] = item.PutInBasket();

            return new ShoppingListSection(Id, items.Values);
        }

        public IShoppingListSection RemoveItemFromBasket(ItemId itemId)
        {
            if (!shoppingListItems.ContainsKey(itemId))
                throw new DomainException(new ItemNotInSectionReason(itemId, Id));

            var items = new Dictionary<ItemId, IShoppingListItem>(shoppingListItems);
            var item = items[itemId];
            items[itemId] = item.RemoveFromBasket();

            return new ShoppingListSection(Id, items.Values);
        }

        public IShoppingListSection ChangeItemQuantity(ItemId itemId, float quantity)
        {
            if (!shoppingListItems.ContainsKey(itemId))
                throw new DomainException(new ItemNotInSectionReason(itemId, Id));

            var items = new Dictionary<ItemId, IShoppingListItem>(shoppingListItems);
            var item = items[itemId];
            items[itemId] = item.ChangeQuantity(quantity);

            return new ShoppingListSection(Id, items.Values);
        }

        public IShoppingListSection RemoveItemsInBasket()
        {
            var items = shoppingListItems.Values
                .Where(i => !i.IsInBasket)
                .ToList();

            return new ShoppingListSection(Id, items);
        }

        public IShoppingListSection RemoveItemsNotInBasket()
        {
            var items = shoppingListItems.Values
                .Where(i => i.IsInBasket)
                .ToList();

            return new ShoppingListSection(Id, items);
        }
    }
}