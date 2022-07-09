﻿using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;

public interface IShoppingListItemFactory
{
    IShoppingListItem Create(ItemId id, ItemTypeId? typeId, bool isInBasket, QuantityInBasket quantity);
}