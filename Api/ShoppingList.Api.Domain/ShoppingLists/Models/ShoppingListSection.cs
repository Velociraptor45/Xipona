using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.ErrorReasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

public class ShoppingListSection : IShoppingListSection
{
    private readonly Dictionary<(ItemId, ItemTypeId?), IShoppingListItem> _shoppingListItems;

    public ShoppingListSection(SectionId id, IEnumerable<IShoppingListItem> shoppingListItems)
    {
        Id = id;
        _shoppingListItems = shoppingListItems.ToDictionary(i => (i.Id, i.TypeId));
    }

    public SectionId Id { get; }

    public IReadOnlyCollection<IShoppingListItem> Items => _shoppingListItems.Values.ToList().AsReadOnly();

    public IShoppingListSection RemoveItem(ItemId itemId)
    {
        return RemoveItem(itemId, null);
    }

    public IShoppingListSection RemoveItem(ItemId itemId, ItemTypeId? itemTypeId)
    {
        if (!_shoppingListItems.ContainsKey((itemId, itemTypeId)))
            return this;

        var items = new Dictionary<(ItemId, ItemTypeId?), IShoppingListItem>(_shoppingListItems);
        items.Remove((itemId, itemTypeId));

        return new ShoppingListSection(Id, items.Values);
    }

    public bool ContainsItem(ItemId itemId)
    {
        return ContainsItem(itemId, null);
    }

    public bool ContainsItem(ItemId itemId, ItemTypeId? itemTypeId)
    {
        return _shoppingListItems.ContainsKey((itemId, itemTypeId));
    }

    public IShoppingListSection AddItem(IShoppingListItem item)
    {
        var items = new Dictionary<(ItemId, ItemTypeId?), IShoppingListItem>(_shoppingListItems);

        if (items.ContainsKey((item.Id, null)))
            throw new DomainException(new ItemAlreadyInSectionReason(item.Id, Id));

        items.Add((item.Id, null), item);
        return new ShoppingListSection(Id, items.Values);
    }

    public IShoppingListSection PutItemInBasket(ItemId itemId, ItemTypeId? itemTypeId)
    {
        if (!_shoppingListItems.ContainsKey((itemId, itemTypeId)))
            throw new DomainException(new ItemNotInSectionReason(itemId, Id));

        var items = new Dictionary<(ItemId, ItemTypeId?), IShoppingListItem>(_shoppingListItems);
        var item = items[(itemId, itemTypeId)];
        items[(itemId, itemTypeId)] = item.PutInBasket();

        return new ShoppingListSection(Id, items.Values);
    }

    public IShoppingListSection RemoveItemFromBasket(ItemId itemId, ItemTypeId? itemTypeId)
    {
        if (!_shoppingListItems.ContainsKey((itemId, itemTypeId)))
            throw new DomainException(new ItemNotInSectionReason(itemId, Id));

        var items = new Dictionary<(ItemId, ItemTypeId?), IShoppingListItem>(_shoppingListItems);
        var item = items[(itemId, itemTypeId)];
        items[(itemId, itemTypeId)] = item.RemoveFromBasket();

        return new ShoppingListSection(Id, items.Values);
    }

    public IShoppingListSection ChangeItemQuantity(ItemId itemId, ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        if (!_shoppingListItems.ContainsKey((itemId, itemTypeId)))
            throw new DomainException(new ItemNotInSectionReason(itemId, Id));

        var items = new Dictionary<(ItemId, ItemTypeId?), IShoppingListItem>(_shoppingListItems);
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