using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models
{
    public class ShoppingListItem
    {
        public ShoppingListItem(ShoppingListItemId id, string name, bool isDeleted, string comment, bool isTemporary,
            float pricePerQuantity, QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            IItemCategory itemCategory, IManufacturer manufacturer, bool isInBasket, float quantity)
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
            IsInBasket = isInBasket;
            Quantity = quantity;
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
        public IItemCategory ItemCategory { get; }
        public IManufacturer Manufacturer { get; }
        public bool IsInBasket { get; private set; }
        public float Quantity { get; private set; }

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