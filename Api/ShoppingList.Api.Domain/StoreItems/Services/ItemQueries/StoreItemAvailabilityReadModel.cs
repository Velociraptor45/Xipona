using ProjectHermes.ShoppingList.Api.Domain.Stores.Queries.AllActiveStores;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemQueries;

public class StoreItemAvailabilityReadModel
{
    public StoreItemAvailabilityReadModel(StoreItemStoreReadModel store, float price,
        StoreSectionReadModel defaultSection)
    {
        Store = store;
        Price = price;
        DefaultSection = defaultSection;
    }

    public StoreItemStoreReadModel Store { get; }
    public float Price { get; }
    public StoreSectionReadModel DefaultSection { get; }
}