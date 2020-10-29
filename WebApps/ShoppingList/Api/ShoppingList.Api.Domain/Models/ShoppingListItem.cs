using System;

namespace ShoppingList.Api.Domain.Models
{
    public class ShoppingListItem
    {
        private bool isInBasket;
        private float quantity;

        public ShoppingListItem(ShoppingListItemId id, string name, bool isDeleted, string comment, bool isTemporary,
            float pricePerQuantity, QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategory itemCategory, Manufacturer manufacturer, bool isInBasket, float quantity)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            PricePerQuantity = pricePerQuantity;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            this.isInBasket = isInBasket;
            this.quantity = quantity;
        }

        public ShoppingListItemId Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public float PricePerQuantity { get; }
        public QuantityType QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityTypeInPacket QuantityTypeInPacket { get; }
        public ItemCategory ItemCategory { get; }
        public Manufacturer Manufacturer { get; }
        public bool IsInBasket => isInBasket;
        public float Quantity => quantity;
        public int DefaultQuantity => QuantityType == QuantityType.Unit ? 1 : 100;
        public string QuantityLabel => QuantityType == QuantityType.Unit ? "" : "g";

        public string PriceLabel => QuantityTypeInPacket switch
        {
            QuantityTypeInPacket.Unit => "€",
            QuantityTypeInPacket.Weight => "€/kg",
            QuantityTypeInPacket.Fluid => "€/l",
            _ => throw new InvalidOperationException(
                $"{nameof(QuantityTypeInPacket)} value {QuantityTypeInPacket} not recognized")
        };

        public void PutInBasket()
        {
            isInBasket = true;
        }

        public void RemoveFromBasket()
        {
            isInBasket = false;
        }

        public void ChangeQuantity(float quantity)
        {
            this.quantity = quantity;
        }
    }
}