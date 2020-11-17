namespace ShoppingList.Api.Contracts.SharedContracts
{
    public class ShoppingListItemContract
    {
        public ShoppingListItemContract(int id, string name, bool isDeleted, string comment, bool isTemporary,
            float pricePerQuantity, int quantityType, float quantityInPacket, int quantityTypeInPacket,
            int defaultQuantity, string quantityLabel, string priceLabel,
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
            DefaultQuantity = defaultQuantity;
            QuantityLabel = quantityLabel;
            PriceLabel = priceLabel;
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
        public int QuantityType { get; }
        public float QuantityInPacket { get; }
        public int QuantityTypeInPacket { get; }
        public int DefaultQuantity { get; }
        public string QuantityLabel { get; }
        public string PriceLabel { get; }
        public ItemCategoryContract ItemCategory { get; }
        public ManufacturerContract Manufacturer { get; }
        public bool IsInBasket { get; }
        public float Quantity { get; }
    }
}