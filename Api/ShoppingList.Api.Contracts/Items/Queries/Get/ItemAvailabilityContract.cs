namespace ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get
{
    public class ItemAvailabilityContract
    {
        public ItemAvailabilityContract(ItemStoreContract store, float price,
            ItemSectionContract defaultSection)
        {
            Store = store;
            Price = price;
            DefaultSection = defaultSection;
        }

        public ItemStoreContract Store { get; }
        public float Price { get; }
        public ItemSectionContract DefaultSection { get; }
    }
}