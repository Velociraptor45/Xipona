using ProjectHermes.ShoppingList.Frontend.Models.Index.Services;
using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class ShoppingListItem
    {
        public ShoppingListItem(ItemId id, Guid? typeId, string name, bool isTemporary, float pricePerQuantity, QuantityType quantityType,
            float? quantityInPacket, QuantityTypeInPacket quantityInPacketType, string itemCategory,
            string manufacturer, bool isInBasket, float quantity)
        {
            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            TypeId = typeId;
            Name = name;
            IsTemporary = isTemporary;
            PricePerQuantity = pricePerQuantity;
            QuantityType = quantityType ?? throw new ArgumentNullException(nameof(quantityType));
            QuantityInPacket = quantityInPacket;
            QuantityInPacketType = quantityInPacketType;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            IsInBasket = isInBasket;
            Quantity = quantity;
        }

        public ItemId Id { get; }
        public Guid? TypeId { get; }
        public string Name { get; }
        public bool IsTemporary { get; }
        public float PricePerQuantity { get; }
        public QuantityType QuantityType { get; }
        public float? QuantityInPacket { get; }
        public QuantityTypeInPacket QuantityInPacketType { get; }
        public string ItemCategory { get; }
        public string Manufacturer { get; }
        public bool IsInBasket { get; set; }
        public float Quantity { get; set; }

        public float TotalPrice => PricePerQuantity * Quantity;

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

        public float GetTotalPrice(IItemPriceCalculationService priceCalculationService)
        {
            return priceCalculationService.CalculatePrice(QuantityType.Id, PricePerQuantity, Quantity);
        }
    }
}