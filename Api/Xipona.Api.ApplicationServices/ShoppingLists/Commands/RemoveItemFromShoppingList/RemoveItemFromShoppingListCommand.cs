using Xipona.Api.ApplicationServices.Common.Commands;
using Xipona.Api.Domain.Items.Models;
using Xipona.Api.Domain.ShoppingLists.Models;
using Xipona.Api.Domain.ShoppingLists.Services.Shared;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Commands.RemoveItemFromShoppingList;

public class RemoveItemFromShoppingListCommand : ICommand<bool>
{
    public RemoveItemFromShoppingListCommand(ShoppingListId shoppingListId, OfflineTolerantItemId itemId,
        ItemTypeId? itemTypeId)
    {
        ShoppingListId = shoppingListId;
        OfflineTolerantItemId = itemId;
        ItemTypeId = itemTypeId;
    }

    public ShoppingListId ShoppingListId { get; }
    public OfflineTolerantItemId OfflineTolerantItemId { get; }
    public ItemTypeId? ItemTypeId { get; }
}