using System.Collections.Generic;

namespace ShoppingList.Frontend.Models.Items
{
    public class StoreItem
    {
        public StoreItem(int id, string name, string comment, bool isTemporary, int quantityType,
            float quantityInPacket, int quantityInPacketType, int itemCategoryId, int manufacturerId,
            IEnumerable<StoreItemAvailabilities> availabilities)
        {
            Id = id;
            Name = name;
            Comment = comment;
            IsTemporary = isTemporary;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityInPacketType = quantityInPacketType;
            ItemCategoryId = itemCategoryId;
            ManufacturerId = manufacturerId;
            Availabilities = availabilities;
        }

        public int Id { get; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public bool IsTemporary { get; set; }
        public int QuantityType { get; set; }
        public float QuantityInPacket { get; set; }
        public int QuantityInPacketType { get; set; }
        public int ItemCategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public IEnumerable<StoreItemAvailabilities> Availabilities { get; set; }
    }
}