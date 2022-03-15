using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Commands.PutItemInBasket;

public class PutItemInBasketCommand : ICommand<bool>
{
    public PutItemInBasketCommand(ShoppingListId shoppingListId, OfflineTolerantItemId itemId,
        ItemTypeId? itemTypeId)
    {
        ShoppingListId = shoppingListId;
        OfflineTolerantItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
        ItemTypeId = itemTypeId;
    }

    public ShoppingListId ShoppingListId { get; }
    public OfflineTolerantItemId OfflineTolerantItemId { get; }
    public ItemTypeId? ItemTypeId { get; }
}