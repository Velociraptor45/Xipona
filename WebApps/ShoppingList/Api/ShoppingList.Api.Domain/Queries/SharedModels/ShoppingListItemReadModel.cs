using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.AllQuantityInPacketTypes;
using ShoppingList.Api.Domain.Queries.AllQuantityTypes;

namespace ShoppingList.Api.Domain.Queries.SharedModels
{
    public class ShoppingListItemReadModel
    {
        public ShoppingListItemReadModel(ShoppingListItemId id, string name, bool isDeleted, string comment,
            bool isTemporary, float pricePerQuantity, QuantityTypeReadModel quantityType, float quantityInPacket,
            QuantityInPacketTypeReadModel quantityTypeInPacket,
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
        public QuantityTypeReadModel QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityInPacketTypeReadModel QuantityTypeInPacket { get; }
        public ItemCategoryReadModel ItemCategory { get; }
        public ManufacturerReadModel Manufacturer { get; }
        public bool IsInBasket { get; }
        public float Quantity { get; }
    }
}