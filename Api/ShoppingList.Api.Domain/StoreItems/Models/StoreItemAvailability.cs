namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemAvailability : IStoreItemAvailability
    {
        public StoreItemAvailability(StoreItemStoreId StoreId, float price, IStoreItemSection defaultSection)
        {
            this.StoreId = StoreId ?? throw new System.ArgumentNullException(nameof(StoreId));
            Price = price;
            DefaultSection = defaultSection;
        }

        public StoreItemStoreId StoreId { get; }
        public float Price { get; }
        public IStoreItemSection DefaultSection { get; }
    }
}