using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;

public class ChangeItemQuantityOnShoppingListCommand : ICommand<bool>
{
    public ChangeItemQuantityOnShoppingListCommand(ShoppingListId shoppingListId, OfflineTolerantItemId itemId,
        ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        ShoppingListId = shoppingListId;
        OfflineTolerantItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
        ItemTypeId = itemTypeId;
        Quantity = quantity;
    }

    public ShoppingListId ShoppingListId { get; }
    public OfflineTolerantItemId OfflineTolerantItemId { get; }
    public ItemTypeId? ItemTypeId { get; }
    public QuantityInBasket Quantity { get; }
}