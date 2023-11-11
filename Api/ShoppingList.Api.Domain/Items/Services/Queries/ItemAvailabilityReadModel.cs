using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

public class ItemAvailabilityReadModel
{
    public ItemAvailabilityReadModel(ItemStoreReadModel store, Price price,
        ItemSectionReadModel defaultSection)
    {
        Store = store;
        Price = price;
        DefaultSection = defaultSection;
    }

    public ItemAvailabilityReadModel(ItemAvailability availability, IStore store, ISection section) :
        this(new ItemStoreReadModel(store), availability.Price, new ItemSectionReadModel(section))
    {
    }

    public ItemStoreReadModel Store { get; }
    public Price Price { get; }
    public ItemSectionReadModel DefaultSection { get; }
}