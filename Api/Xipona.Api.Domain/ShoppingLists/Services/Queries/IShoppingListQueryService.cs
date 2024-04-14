using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Queries;

public interface IShoppingListQueryService
{
    Task<ShoppingListReadModel> GetActiveAsync(StoreId storeId);
}