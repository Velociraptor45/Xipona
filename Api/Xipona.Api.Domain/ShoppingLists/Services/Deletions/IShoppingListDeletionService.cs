using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.ShoppingLists.Services.Deletions;

public interface IShoppingListDeletionService
{
    Task HardDeleteForStoreAsync(StoreId storeId);
}