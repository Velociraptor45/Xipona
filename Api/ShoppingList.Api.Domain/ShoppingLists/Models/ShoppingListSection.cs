using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

public class ShoppingListSection : IShoppingListSection
{
    private readonly Dictionary<(ItemId, ItemTypeId?), ShoppingListItem> _shoppingListItems;

    public ShoppingListSection(SectionId id, IEnumerable<ShoppingListItem> shoppingListItems)
    {
        Id = id;
        _shoppingListItems = shoppingListItems.ToDictionary(i => (i.Id, i.TypeId));
    }

    public SectionId Id { get; }

    public IReadOnlyCollection<ShoppingListItem> Items => _shoppingListItems.Values.ToList().AsReadOnly();

    public IShoppingListSection RemoveItemAndItsTypes(ItemId itemId)
    {
        if (!ContainsItemOrItsTypes(itemId))
            return this;

        var relevantItems = _shoppingListItems.Where(i => i.Key.Item1 != itemId);
        var items = new Dictionary<(ItemId, ItemTypeId?), ShoppingListItem>(relevantItems);

        return new ShoppingListSection(Id, items.Values);
    }

    public IShoppingListSection RemoveItem(ItemId itemId)
    {
        return RemoveItem(itemId, null);
    }

    public IShoppingListSection RemoveItem(ItemId itemId, ItemTypeId? itemTypeId)
    {
        if (!_shoppingListItems.ContainsKey((itemId, itemTypeId)))
            return this;

        var items = new Dictionary<(ItemId, ItemTypeId?), ShoppingListItem>(_shoppingListItems);
        items.Remove((itemId, itemTypeId));

        return new ShoppingListSection(Id, items.Values);
    }

    public bool ContainsItemOrItsTypes(ItemId itemId)
    {
        return _shoppingListItems.Keys.Any(k => k.Item1 == itemId);
    }

    public bool ContainsItem(ItemId itemId)
    {
        return ContainsItem(itemId, null);
    }

    public bool ContainsItem(ItemId itemId, ItemTypeId? itemTypeId)
    {
        return _shoppingListItems.ContainsKey((itemId, itemTypeId));
    }

    public IShoppingListSection AddItem(ShoppingListItem item, bool throwIfAlreadyPresent = true)
    {
        var items = new Dictionary<(ItemId, ItemTypeId?), ShoppingListItem>(_shoppingListItems);

        var itemAlreadyPresent = items.ContainsKey((item.Id, item.TypeId));
        if (itemAlreadyPresent)
        {
            if (throwIfAlreadyPresent)
                throw new DomainException(new ItemAlreadyInSectionReason(item.Id, Id));

            items[(item.Id, item.TypeId)] = items[(item.Id, item.TypeId)].AddQuantity(item.Quantity);
        }
        else
        {
            items.Add((item.Id, item.TypeId), item);
        }

        return new ShoppingListSection(Id, items.Values);
    }

    public IShoppingListSection PutItemInBasket(ItemId itemId, ItemTypeId? itemTypeId)
    {
        if (!_shoppingListItems.ContainsKey((itemId, itemTypeId)))
            throw new DomainException(new ItemNotInSectionReason(itemId, Id));

        var items = new Dictionary<(ItemId, ItemTypeId?), ShoppingListItem>(_shoppingListItems);
        var item = items[(itemId, itemTypeId)];
        items[(itemId, itemTypeId)] = item.PutInBasket();

        return new ShoppingListSection(Id, items.Values);
    }

    public IShoppingListSection RemoveItemFromBasket(ItemId itemId, ItemTypeId? itemTypeId)
    {
        if (!_shoppingListItems.ContainsKey((itemId, itemTypeId)))
            throw new DomainException(new ItemNotInSectionReason(itemId, Id));

        var items = new Dictionary<(ItemId, ItemTypeId?), ShoppingListItem>(_shoppingListItems);
        var item = items[(itemId, itemTypeId)];
        items[(itemId, itemTypeId)] = item.RemoveFromBasket();

        return new ShoppingListSection(Id, items.Values);
    }

    public IShoppingListSection ChangeItemQuantity(ItemId itemId, ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        if (!_shoppingListItems.ContainsKey((itemId, itemTypeId)))
            throw new DomainException(new ItemNotInSectionReason(itemId, Id));

        var items = new Dictionary<(ItemId, ItemTypeId?), ShoppingListItem>(_shoppingListItems);
        var item = items[(itemId, itemTypeId)];
        items[(itemId, itemTypeId)] = item.ChangeQuantity(quantity);

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