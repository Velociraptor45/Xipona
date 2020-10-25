using System;
using System.Collections.Generic;

namespace ShoppingList.Database.Entities
{
    public partial class QuantityType
    {
        public QuantityType()
        {
            ItemQuantityInPacketType = new HashSet<Item>();
            ItemQuantityType = new HashSet<Item>();
        }

        public uint QuantityTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Item> ItemQuantityInPacketType { get; set; }
        public virtual ICollection<Item> ItemQuantityType { get; set; }
    }
}
