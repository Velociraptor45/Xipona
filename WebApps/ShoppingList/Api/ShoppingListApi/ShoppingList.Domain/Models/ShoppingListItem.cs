namespace ShoppingList.Domain.Models
{
    public class ShoppingListItem
    {
        public ShoppingListItem(ShoppingListItemId id, string name, bool isDeleted, string comment, bool isTemporary, float price,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategory itemCategory, Manufacturer manufacturer)
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
        }

        public ShoppingListItemId Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public float Price { get; }
        public QuantityType QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityTypeInPacket QuantityTypeInPacket { get; }
        public ItemCategory ItemCategory { get; }
        public Manufacturer Manufacturer { get; }
    }
}