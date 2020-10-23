namespace ShoppingList.Domain.Models
{
    public class StoreItem
    {
        private StoreItemId id;

        public StoreItem(StoreItemId id, string name, bool isDeleted, string comment, bool isTemporary, float price,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategoryId itemCategoryId, ManufacturerId manufacturerId, StoreId storeId)
        {
            this.id = id;
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            Price = price;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategoryId = itemCategoryId;
            ManufacturerId = manufacturerId;
            StoreId = storeId;
        }

        public StoreItemId Id { get => id; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public float Price { get; }
        public QuantityType QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityTypeInPacket QuantityTypeInPacket { get; }
        public ItemCategoryId ItemCategoryId { get; }
        public ManufacturerId ManufacturerId { get; }
        public StoreId StoreId { get; }

        public void ChangeId(StoreItemId id)
        {
            this.id = id;
        }
    }
}