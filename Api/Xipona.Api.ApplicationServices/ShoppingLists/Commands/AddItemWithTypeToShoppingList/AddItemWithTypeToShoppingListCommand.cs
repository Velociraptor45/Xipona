﻿using ProjectHermes.Xipona.Api.ApplicationServices.Common.Commands;
using ProjectHermes.Xipona.Api.Domain.Items.Models;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Commands.AddItemWithTypeToShoppingList;

public class AddItemWithTypeToShoppingListCommand : ICommand<bool>
{
    public AddItemWithTypeToShoppingListCommand(ShoppingListId shoppingListId, ItemId itemId,
        ItemTypeId itemTypeId, SectionId? sectionId, QuantityInBasket quantity)
    {
        ShoppingListId = shoppingListId;
        ItemId = itemId;
        ItemTypeId = itemTypeId;
        SectionId = sectionId;
        Quantity = quantity;
    }

    public ShoppingListId ShoppingListId { get; }
    public ItemId ItemId { get; }
    public ItemTypeId ItemTypeId { get; }
    public SectionId? SectionId { get; }
    public QuantityInBasket Quantity { get; }
}