using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingListSection : IShoppingListSection
    {
        private readonly Dictionary<ItemId, IShoppingListItem> _shoppingListItems;

        public ShoppingListSection(SectionId id, IEnumerable<IShoppingListItem> shoppingListItems)
        {
            Id = id;
            _shoppingListItems = shoppingListItems.ToDictionary(i => i.Id);
        }

        public SectionId Id { get; }

        public IReadOnlyCollection<IShoppingListItem> Items => _shoppingListItems.Values.ToList().AsReadOnly();

        public IShoppingListSection RemoveItem(ItemId itemId)
        {
            if (!_shoppingListItems.ContainsKey(itemId))
                return this;

            var items = new Dictionary<ItemId, IShoppingListItem>(_shoppingListItems);
            items.Remove(itemId);

            return new ShoppingListSection(Id, items.Values);
        }

        public bool ContainsItem(ItemId itemId)
        {
            return _shoppingListItems.ContainsKey(itemId);
        }

        public IShoppingListSection AddItem(IShoppingListItem item)
        {
            var items = new Dictionary<ItemId, IShoppingListItem>(_shoppingListItems);

            if (items.ContainsKey(item.Id))
                throw new DomainException(new ItemAlreadyInSectionReason(item.Id, Id));

            items.Add(item.Id, item);
            return new ShoppingListSection(Id, items.Values);
        }

        public IShoppingListSection PutItemInBasket(ItemId itemId)
        {
            if (!_shoppingListItems.ContainsKey(itemId))
                throw new DomainException(new ItemNotInSectionReason(itemId, Id));

            var items = new Dictionary<ItemId, IShoppingListItem>(_shoppingListItems);
            var item = items[itemId];
            items[itemId] = item.PutInBasket();

            return new ShoppingListSection(Id, items.Values);
        }

        public IShoppingListSection RemoveItemFromBasket(ItemId itemId)
        {
            if (!_shoppingListItems.ContainsKey(itemId))
                throw new DomainException(new ItemNotInSectionReason(itemId, Id));

            var items = new Dictionary<ItemId, IShoppingListItem>(_shoppingListItems);
            var item = items[itemId];
            items[itemId] = item.RemoveFromBasket();

            return new ShoppingListSection(Id, items.Values);
        }

        public IShoppingListSection ChangeItemQuantity(ItemId itemId, float quantity)
        {
            if (!_shoppingListItems.ContainsKey(itemId))
                throw new DomainException(new ItemNotInSectionReason(itemId, Id));

            var items = new Dictionary<ItemId, IShoppingListItem>(_shoppingListItems);
            var item = items[itemId];
            items[itemId] = item.ChangeQuantity(quantity);

            return new ShoppingListSection(Id, items.Values);
        }

        public IShoppingListSection RemoveItemsInBasket()
        {
            var items = _shoppingListItems.Values
                .Where(i => !i.IsInBasket)
                .ToList();

            return new ShoppingListSection(Id, items);
        }

        public IShoppingListSection RemoveItemsNotInBasket()
        {
            var items = _shoppingListItems.Values
                .Where(i => i.IsInBasket)
                .ToList();

            return new ShoppingListSection(Id, items);
        }
    }
}