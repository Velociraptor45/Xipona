using System;
using System.Collections.Generic;

namespace ShoppingList.Database.Entities
{
    public partial class QuantityType
    {
        public QuantityType()
        {
            Item = new HashSet<Item>();
        }

        public uint QuantityTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Item> Item { get; set; }
    }
}
