using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items
{
    public class StoreItem
    {
        public StoreItem(int id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityInPacketType,
            int itemCategoryId, int manufacturerId, IEnumerable<StoreItemAvailability> availabilities)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityInPacketType = quantityInPacketType;
            ItemCategoryId = itemCategoryId;
            ManufacturerId = manufacturerId;
            Availabilities = availabilities.ToList();
        }

        public int Id { get; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string Comment { get; set; }
        public bool IsTemporary { get; set; }
        public QuantityType QuantityType { get; set; }
        public float QuantityInPacket { get; set; }
        public QuantityTypeInPacket QuantityInPacketType { get; set; }
        public int ItemCategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public List<StoreItemAvailability> Availabilities { get; set; }
    }
}