﻿using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;

public interface IShoppingListSection
{
    SectionId Id { get; }
    public IReadOnlyCollection<ShoppingListItem> Items { get; }

    IShoppingListSection AddItem(ShoppingListItem item, bool throwIfAlreadyPresent = true);

    IShoppingListSection ChangeItemQuantity(ItemId itemId, ItemTypeId? itemTypeId, QuantityInBasket quantity);

    bool ContainsItem(ItemId itemId);

    bool ContainsItem(ItemId itemId, ItemTypeId? itemTypeId);

    IShoppingListSection PutItemInBasket(ItemId itemId, ItemTypeId? itemTypeId);

    IShoppingListSection RemoveItemFromBasket(ItemId itemId, ItemTypeId? itemTypeId);

    IShoppingListSection RemoveItem(ItemId itemId);

    IShoppingListSection RemoveItem(ItemId itemId, ItemTypeId? itemTypeId);

    IShoppingListSection RemoveItemsInBasket();

    IShoppingListSection RemoveItemsNotInBasket();

    bool ContainsItemOrItsTypes(ItemId itemId);

    IShoppingListSection RemoveItemAndItsTypes(ItemId itemId);
}