using ShoppingList.Api.Contracts.Queries.AllQuantityTypes;

namespace ShoppingList.Api.Contracts.SharedContracts
{
    public class ShoppingListItemContract
    {
        public ShoppingListItemContract(int id, string name, bool isDeleted, string comment, bool isTemporary,
            float pricePerQuantity, QuantityTypeContract quantityType, float quantityInPacket,
            QuantityInPacketTypeContract quantityTypeInPacket,
            ItemCategoryContract itemCategory, ManufacturerContract manufacturer, bool isInBasket, float quantity)
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

        public int Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public float PricePerQuantity { get; }
        public QuantityTypeContract QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityInPacketTypeContract QuantityTypeInPacket { get; }
        public ItemCategoryContract ItemCategory { get; }
        public ManufacturerContract Manufacturer { get; }
        public bool IsInBasket { get; }
        public float Quantity { get; }
    }
}