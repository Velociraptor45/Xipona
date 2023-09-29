using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Deletions;

public interface IShoppingListDeletionService
{
    Task HardDeleteForStoreAsync(StoreId storeId);
}