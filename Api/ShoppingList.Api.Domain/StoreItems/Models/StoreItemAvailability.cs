namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public class StoreItemAvailability : IStoreItemAvailability
    {
        // pass just default section id and search for it when property is accessed
        public StoreItemAvailability(IStoreItemStore store, float price, IStoreItemSection defaultSection)
        {
            Store = store ?? throw new System.ArgumentNullException(nameof(store));
            Price = price;
            DefaultSection = defaultSection ?? throw new System.ArgumentNullException(nameof(defaultSection));
        }

        public IStoreItemStore Store { get; }
        public float Price { get; }
        public IStoreItemSection DefaultSection { get; }
    }
}