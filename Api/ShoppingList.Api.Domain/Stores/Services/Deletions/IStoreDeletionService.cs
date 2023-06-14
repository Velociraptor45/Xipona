using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Deletions;

public interface IStoreDeletionService
{
    Task DeleteAsync(StoreId storeId);
}