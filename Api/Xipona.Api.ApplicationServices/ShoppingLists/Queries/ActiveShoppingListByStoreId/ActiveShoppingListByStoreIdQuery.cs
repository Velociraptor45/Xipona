using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.ShoppingLists.Services.Queries;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.ApplicationServices.ShoppingLists.Queries.ActiveShoppingListByStoreId;

public class ActiveShoppingListByStoreIdQuery : IQuery<ShoppingListReadModel>
{
    public ActiveShoppingListByStoreIdQuery(StoreId storeId)
    {
        StoreId = storeId;
    }

    public StoreId StoreId { get; }
}