using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemAvailability : IStoreItemAvailability
    {
        public StoreItemAvailability(StoreId StoreId, float price)
        {
            this.StoreId = StoreId ?? throw new System.ArgumentNullException(nameof(StoreId));
            Price = price;
        }

        public StoreId StoreId { get; }
        public float Price { get; }
    }
}