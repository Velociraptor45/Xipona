using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items.Models
{
    public class Item : IAvailable
    {
        public Item(Guid id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityType quantityType, float? quantityInPacket, QuantityTypeInPacket quantityInPacketType,
            Guid? itemCategoryId, Guid? manufacturerId, IEnumerable<ItemAvailability> availabilities,
            IEnumerable<ItemType> itemTypes)
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
            ItemTypes = itemTypes.ToList();
            Availabilities = availabilities.ToList();
            SetItemMode();
        }

        public Guid Id { get; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string Comment { get; set; }
        public bool IsTemporary { get; set; }
        public QuantityType QuantityType { get; set; }
        public float? QuantityInPacket { get; set; }
        public QuantityTypeInPacket QuantityInPacketType { get; set; }
        public Guid? ItemCategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public List<ItemType> ItemTypes { get; set; }
        public List<ItemAvailability> Availabilities { get; set; }
        public ItemMode ItemMode { get; private set; }

        public bool IsItemWithTypes => ItemMode == ItemMode.WithTypes;

        private void SetItemMode()
        {
            if (Id == Guid.Empty)
            {
                ItemMode = ItemMode.NotDefined;
                return;
            }

            ItemMode = ItemTypes.Count > 0 ? ItemMode.WithTypes : ItemMode.WithoutTypes;
        }
    }
}