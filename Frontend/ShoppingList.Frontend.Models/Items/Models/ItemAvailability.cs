using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items.Models
{
    public class ItemAvailability
    {
        public ItemStore Store { get; set; }
        public float PricePerQuantity { get; set; }
        public Guid DefaultSectionId { get; set; }
    }
}