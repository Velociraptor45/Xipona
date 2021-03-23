using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemAvailability : IStoreItemAvailability
    {
        public StoreItemAvailability(StoreId storeId, float price, SectionId defaultSectionId)
        {
            StoreId = storeId ?? throw new System.ArgumentNullException(nameof(storeId));
            Price = price;
            DefaultSectionId = defaultSectionId ?? throw new System.ArgumentNullException(nameof(defaultSectionId));
        }

        public StoreId StoreId { get; }
        public float Price { get; }
        public SectionId DefaultSectionId { get; }
    }
}