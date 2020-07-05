using System;
using System.Collections.Generic;

namespace ShoppingList.Database.Entities
{
    public partial class Item
    {
        public Item()
        {
            ItemOnShoppingList = new HashSet<ItemOnShoppingList>();
        }

        public uint ItemId { get; set; }
        public uint? ItemCategoryId { get; set; }
        public uint? ManufacturerId { get; set; }
        public string Name { get; set; }
        public uint QuantityTypeId { get; set; }
        public float QuantityInPacket { get; set; }
        public decimal PricePerQuantity { get; set; }
        public bool? Active { get; set; }
        public uint StoreId { get; set; }
        public string Comment { get; set; }
        public uint QuantityInPacketTypeId { get; set; }
        public bool IsTemporary { get; set; }

        public virtual ItemCategory ItemCategory { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual QuantityType QuantityInPacketType { get; set; }
        public virtual QuantityType QuantityType { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<ItemOnShoppingList> ItemOnShoppingList { get; set; }
    }
}
