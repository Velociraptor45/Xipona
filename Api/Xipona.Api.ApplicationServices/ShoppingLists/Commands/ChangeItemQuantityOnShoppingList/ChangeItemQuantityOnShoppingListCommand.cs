using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.ShoppingLists.Services.Shared;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;

public class ChangeItemQuantityOnShoppingListCommand : ICommand<bool>
{
    public ChangeItemQuantityOnShoppingListCommand(ShoppingListId shoppingListId, OfflineTolerantItemId itemId,
        ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        ShoppingListId = shoppingListId;
        OfflineTolerantItemId = itemId;
        ItemTypeId = itemTypeId;
        Quantity = quantity;
    }

    public ShoppingListId ShoppingListId { get; }
    public OfflineTolerantItemId OfflineTolerantItemId { get; }
    public ItemTypeId? ItemTypeId { get; }
    public QuantityInBasket Quantity { get; }
}