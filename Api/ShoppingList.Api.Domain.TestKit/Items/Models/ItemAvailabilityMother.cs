using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ShoppingList.Api.Domain.TestKit.Items.Models;

public static class ItemAvailabilityMother
{
    public static ItemAvailabilityBuilder Initial()
    {
        return new ItemAvailabilityBuilder();
    }

    public static ItemAvailabilityBuilder ForStore(StoreId storeId)
    {
        return new ItemAvailabilityBuilder().WithStoreId(storeId);
    }
}