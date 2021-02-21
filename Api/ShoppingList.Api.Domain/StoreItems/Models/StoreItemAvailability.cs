namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemAvailability : IStoreItemAvailability
    {
        public StoreItemAvailability(IStoreItemStore store, float price, IStoreItemSection defaultSection)
        {
            Store = store ?? throw new System.ArgumentNullException(nameof(store));
            Price = price;
            DefaultSection = defaultSection;
        }

        public IStoreItemStore Store { get; }
        public float Price { get; }
        public IStoreItemSection DefaultSection { get; }
    }
}