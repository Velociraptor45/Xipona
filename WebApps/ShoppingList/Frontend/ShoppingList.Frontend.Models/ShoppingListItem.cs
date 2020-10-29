namespace ShoppingList.Frontend.Models
{
    public class ShoppingListItem
    {
        public ShoppingListItem(int id, string name, bool isTemporary, float pricePerQuantity, float quantityInPacket,
            int defaultQuantity, string itemCategory, string manufacturer, bool isInBasket, float quantity)
        {
            Id = id;
            Name = name;
            IsTemporary = isTemporary;
            PricePerQuantity = pricePerQuantity;
            QuantityInPacket = quantityInPacket;
            DefaultQuantity = defaultQuantity;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            IsInBasket = isInBasket;
            Quantity = quantity;
        }

        public int Id { get; }
        public string Name { get; }
        public bool IsTemporary { get; }
        public float PricePerQuantity { get; }
        public float QuantityInPacket { get; }
        public int DefaultQuantity { get; }
        public string ItemCategory { get; }
        public string Manufacturer { get; }
        public bool IsInBasket { get; set; }
        public float Quantity { get; set; }
    }
}