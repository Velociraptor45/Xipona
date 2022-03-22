using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Models;

public static class StoreItemAvailabilityMother
{
    public static StoreItemAvailabilityBuilder Initial()
    {
        return new StoreItemAvailabilityBuilder();
    }

    public static StoreItemAvailabilityBuilder ForStore(StoreId storeId)
    {
        return new StoreItemAvailabilityBuilder().WithStoreId(storeId);
    }
}