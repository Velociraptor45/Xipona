using ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Domain.Queries.SharedModels
{
    public class ShoppingListItemReadModel
    {
        public ShoppingListItemReadModel(ShoppingListItemId id, string name, bool isDeleted, string comment,
            bool isTemporary, float pricePerQuantity, QuantityType quantityType, float quantityInPacket,
            int defaultQuantity, string quantityLable, string priceLabel, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategoryReadModel itemCategory, ManufacturerReadModel manufacturer,
            bool isInBasket, float quantity)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            PricePerQuantity = pricePerQuantity;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            DefaultQuantity = defaultQuantity;
            QuantityLable = quantityLable;
            PriceLabel = priceLabel;
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
        public int DefaultQuantity { get; }
        public string QuantityLable { get; }
        public string PriceLabel { get; }
        public QuantityTypeInPacket QuantityTypeInPacket { get; }
        public ItemCategoryReadModel ItemCategory { get; }
        public ManufacturerReadModel Manufacturer { get; }
        public bool IsInBasket { get; }
        public float Quantity { get; }
    }
}