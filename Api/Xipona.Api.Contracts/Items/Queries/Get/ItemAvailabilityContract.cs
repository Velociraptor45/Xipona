namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get
{
    public class ItemAvailabilityContract
    {
        public ItemAvailabilityContract(ItemStoreContract store, decimal price,
            ItemSectionContract defaultSection)
        {
            Store = store;
            Price = price;
            DefaultSection = defaultSection;
        }

        public ItemStoreContract Store { get; }
        public decimal Price { get; }
        public ItemSectionContract DefaultSection { get; }
    }
}