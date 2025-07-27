using Xipona.Api.Repositories.Stores.Entities;
using Xipona.Api.Repositories.TestKit.ShoppingLists.Entities;

namespace Xipona.Api.Endpoint.IntegrationTests.v1.Endpoints;
public class ShoppingListEntityCreationContext
{
    private Store? _store;

    public void AddStore(Store store)
    {
        _store = store;
    }

    public ShoppingListEntityBuilder FillShoppingList(ShoppingListEntityBuilder builder)
    {
        if (_store is not null)
            builder.WithStoreId(_store.Id);

        return builder;
    }
}
