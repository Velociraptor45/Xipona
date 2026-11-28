using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.ShoppingLists.Services.Queries;

public class ShoppingListStoreReadModel
{
    public ShoppingListStoreReadModel(StoreId id, StoreName name)
    {
        Id = id;
        Name = name;
    }

    public StoreId Id { get; }
    public StoreName Name { get; }
}