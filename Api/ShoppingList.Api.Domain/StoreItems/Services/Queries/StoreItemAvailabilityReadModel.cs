using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

public class StoreItemAvailabilityReadModel
{
    public StoreItemAvailabilityReadModel(StoreItemStoreReadModel store, float price,
        StoreSectionReadModel defaultSection)
    {
        Store = store;
        Price = price;
        DefaultSection = defaultSection;
    }

    public StoreItemAvailabilityReadModel(IStoreItemAvailability availability, IStore store, IStoreSection section) :
        this(new StoreItemStoreReadModel(store), availability.Price, new StoreSectionReadModel(section))
    {
    }

    public StoreItemStoreReadModel Store { get; }
    public float Price { get; }
    public StoreSectionReadModel DefaultSection { get; }
}