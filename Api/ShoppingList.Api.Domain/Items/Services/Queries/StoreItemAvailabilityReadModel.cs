using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

public class StoreItemAvailabilityReadModel
{
    public StoreItemAvailabilityReadModel(StoreItemStoreReadModel store, Price price,
        StoreItemSectionReadModel defaultSection)
    {
        Store = store;
        Price = price;
        DefaultSection = defaultSection;
    }

    public StoreItemAvailabilityReadModel(IItemAvailability availability, IStore store, IStoreSection section) :
        this(new StoreItemStoreReadModel(store), availability.Price, new StoreItemSectionReadModel(section))
    {
    }

    public StoreItemStoreReadModel Store { get; }
    public Price Price { get; }
    public StoreItemSectionReadModel DefaultSection { get; }
}