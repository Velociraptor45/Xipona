using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.ShoppingLists.Services.Queries;

public interface IShoppingListQueryService
{
    Task<ShoppingListReadModel> GetActiveAsync(StoreId storeId);
}