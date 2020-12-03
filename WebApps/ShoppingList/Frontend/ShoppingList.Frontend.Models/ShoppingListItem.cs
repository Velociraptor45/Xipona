using ShoppingList.Frontend.Models.Shared;

namespace ShoppingList.Frontend.Models
{
    public class ShoppingListItem
    {
        public ShoppingListItem(ItemId id, string name, bool isTemporary, float pricePerQuantity, QuantityType quantityType,
            float quantityInPacket, QuantityInPacketType quantityInPacketType, string itemCategory,
            string manufacturer, bool isInBasket, float quantity)
        {
            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            Name = name;
            IsTemporary = isTemporary;
            PricePerQuantity = pricePerQuantity;
            QuantityType = quantityType ?? throw new System.ArgumentNullException(nameof(quantityType));
            QuantityInPacket = quantityInPacket;
            QuantityInPacketType = quantityInPacketType ?? throw new System.ArgumentNullException(nameof(quantityInPacketType));
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            IsInBasket = isInBasket;
            Quantity = quantity;
        }

        public ItemId Id { get; }
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

        public void PutInBasket()
        {
            IsInBasket = true;
        }

        public void RemoveFromBasket()
        {
            IsInBasket = false;
        }

        public void ChangeQuantity(float quantity)
        {
            Quantity = quantity;
        }
    }
}