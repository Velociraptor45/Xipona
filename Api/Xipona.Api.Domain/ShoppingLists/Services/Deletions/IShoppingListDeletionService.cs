using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Deletions;

public interface IShoppingListDeletionService
{
    Task HardDeleteForStoreAsync(StoreId storeId);
}