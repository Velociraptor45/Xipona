namespace ShoppingList.Frontend.Models
{
    public class ShoppingListItem
    {
        public ShoppingListItem(int id, string name, bool isTemporary, float pricePerQuantity, QuantityType quantityType,
            float quantityInPacket, QuantityInPacketType quantityInPacketType, string itemCategory,
            string manufacturer, bool isInBasket, float quantity)
        {
            Id = id;
            Name = name;
            IsTemporary = isTemporary;
            PricePerQuantity = pricePerQuantity;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityInPacketType = quantityInPacketType;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            IsInBasket = isInBasket;
            Quantity = quantity;
        }

        public int Id { get; }
        public string Name { get; }
        public bool IsTemporary { get; }
        public float PricePerQuantity { get; }
        public QuantityType QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityInPacketType QuantityInPacketType { get; }
        public string ItemCategory { get; }
        public string Manufacturer { get; }
        public bool IsInBasket { get; set; }
        public float Quantity { get; set; }
    }
}