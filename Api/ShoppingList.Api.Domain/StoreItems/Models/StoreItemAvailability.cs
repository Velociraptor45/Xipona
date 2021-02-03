using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemAvailability : IStoreItemAvailability
    {
        public StoreItemAvailability(StoreId StoreId, float price, IStoreItemSection defaultSection)
        {
            this.StoreId = StoreId ?? throw new System.ArgumentNullException(nameof(StoreId));
            Price = price;
            DefaultSection = defaultSection;
        }

        public StoreId StoreId { get; }
        public float Price { get; }
        public IStoreItemSection DefaultSection { get; }
    }
}