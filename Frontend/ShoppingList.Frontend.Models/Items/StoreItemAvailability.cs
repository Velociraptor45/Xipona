namespace ProjectHermes.ShoppingList.Frontend.Models.Items
{
    public class StoreItemAvailability
    {
        public StoreItemAvailability(StoreItemStore store, float pricePerQuantity, StoreItemSection defaultSection)
        {
            Store = store;
            PricePerQuantity = pricePerQuantity;
            DefaultSection = defaultSection;
        }

        public StoreItemStore Store { get; set; }
        public float PricePerQuantity { get; set; }
        public StoreItemSection DefaultSection { get; set; }

        public void ChangeDefaultSection(StoreItemSection section)
        {
            DefaultSection = section;
        }
    }
}