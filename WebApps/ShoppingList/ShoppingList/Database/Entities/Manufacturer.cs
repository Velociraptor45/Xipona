using System;
using System.Collections.Generic;

namespace ShoppingList.Database.Entities
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Item = new HashSet<Item>();
        }

        public uint ManufacturerId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Item> Item { get; set; }
    }
}
