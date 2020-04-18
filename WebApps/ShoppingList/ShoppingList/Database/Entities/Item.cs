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
        public string Name { get; set; }
        public uint QuantityTypeId { get; set; }
        public decimal PricePerQuantity { get; set; }
        public bool Active { get; set; }

        public virtual QuantityType QuantityType { get; set; }
        public virtual ICollection<ItemOnShoppingList> ItemOnShoppingList { get; set; }
    }
}
