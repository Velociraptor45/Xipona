namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels
{
    public class StoreItemAvailabilityReadModel
    {
        public StoreItemAvailabilityReadModel(StoreItemStoreReadModel store, float price,
            StoreItemSectionReadModel defaultSection)
        {
            Store = store;
            Price = price;
            DefaultSection = defaultSection;
        }

        public StoreItemStoreReadModel Store { get; }
        public float Price { get; }
        public StoreItemSectionReadModel DefaultSection { get; }
    }
}