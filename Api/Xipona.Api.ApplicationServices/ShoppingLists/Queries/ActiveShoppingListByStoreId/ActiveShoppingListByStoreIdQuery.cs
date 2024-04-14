using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.ShoppingLists.Services.Queries;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ShoppingLists.Queries.ActiveShoppingListByStoreId;

public class ActiveShoppingListByStoreIdQuery : IQuery<ShoppingListReadModel>
{
    public ActiveShoppingListByStoreIdQuery(StoreId storeId)
    {
        StoreId = storeId;
    }

    public StoreId StoreId { get; }
}