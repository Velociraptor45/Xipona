using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items
{
    public class ItemAvailability
    {
        public ItemAvailability(ItemStore store, float pricePerQuantity, Guid defaultSectionId)
        {
            Store = store;
            PricePerQuantity = pricePerQuantity;
            DefaultSectionId = defaultSectionId;
        }

        public ItemStore Store { get; set; }
        public float PricePerQuantity { get; set; }
        public Guid DefaultSectionId { get; set; }

        public void ChangeDefaultSectionId(Guid sectionId)
        {
            DefaultSectionId = sectionId;
        }
    }
}