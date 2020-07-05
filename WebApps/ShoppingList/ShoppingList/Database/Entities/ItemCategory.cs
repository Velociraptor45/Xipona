using System;
using System.Collections.Generic;

namespace ShoppingList.Database.Entities
{
    public partial class ItemCategory
    {
        public ItemCategory()
        {
            Item = new HashSet<Item>();
        }

        public uint ItemCategoryId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Item> Item { get; set; }
    }
}
