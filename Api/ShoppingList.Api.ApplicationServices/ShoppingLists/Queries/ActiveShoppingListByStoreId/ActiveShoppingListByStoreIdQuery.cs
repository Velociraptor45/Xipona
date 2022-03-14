using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ShoppingLists.Queries.ActiveShoppingListByStoreId;

public class ActiveShoppingListByStoreIdQuery : IQuery<ShoppingListReadModel>
{
    public ActiveShoppingListByStoreIdQuery(StoreId storeId)
    {
        StoreId = storeId;
    }

    public StoreId StoreId { get; }
}