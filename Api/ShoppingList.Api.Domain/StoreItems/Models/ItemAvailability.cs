using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

public class ItemAvailability : IItemAvailability
{
    public ItemAvailability(StoreId storeId, Price price, SectionId defaultSectionId)
    {
        StoreId = storeId;
        Price = price;
        DefaultSectionId = defaultSectionId;
    }

    public StoreId StoreId { get; }
    public Price Price { get; }
    public SectionId DefaultSectionId { get; }
}