namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get
{
    /// <summary>
    /// Represents availability of an item in a store.
    /// </summary>
    public class ItemAvailabilityContract
    {
        /// <summary>
        /// </summary>
        /// <param name="store"></param>
        /// <param name="price"></param>
        /// <param name="defaultSection"></param>
        public ItemAvailabilityContract(ItemStoreContract store, float price,
            ItemSectionContract defaultSection)
        {
            Store = store;
            Price = price;
            DefaultSection = defaultSection;
        }

        /// <summary>
        /// The store where the item is available.
        /// </summary>
        public ItemStoreContract Store { get; }

        /// <summary>
        /// The item's price in the store.
        /// </summary>
        public float Price { get; }

        /// <summary>
        /// The section where the item is normally located in the store.
        /// </summary>
        public ItemSectionContract DefaultSection { get; }
    }
}