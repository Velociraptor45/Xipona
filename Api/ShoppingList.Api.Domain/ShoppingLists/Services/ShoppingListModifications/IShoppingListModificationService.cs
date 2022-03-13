using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.ShoppingListModifications;

public interface IShoppingListModificationService
{
    Task ChangeItemQuantityAsync(ShoppingListId shoppingListId,
        OfflineTolerantItemId offlineTolerantItemId, ItemTypeId? itemTypeId, float quantity);

    Task RemoveItemAsync(ShoppingListId shoppingListId, OfflineTolerantItemId offlineTolerantItemId,
        ItemTypeId? itemTypeId);

    Task RemoveItemFromBasketAsync(ShoppingListId shoppingListId,
        OfflineTolerantItemId offlineTolerantItemId, ItemTypeId? itemTypeId);
}