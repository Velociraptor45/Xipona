namespace ShoppingList.Api.Contracts.SharedContracts
{
    public class ShoppingListItemContract
    {
        public ShoppingListItemContract(int id, string name, bool isDeleted, string comment, bool isTemporary,
            float price, string quantityType, float quantityInPacket, string quantityTypeInPacket,
            ItemCategoryContract itemCategory, ManufacturerContract manufacturer, bool isInBasket, float quantity)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            Price = price;
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
        public float Price { get; }
        public string QuantityType { get; }
        public float QuantityInPacket { get; }
        public string QuantityTypeInPacket { get; }
        public ItemCategoryContract ItemCategory { get; }
        public ManufacturerContract Manufacturer { get; }
        public bool IsInBasket { get; }
        public float Quantity { get; }
    }
}