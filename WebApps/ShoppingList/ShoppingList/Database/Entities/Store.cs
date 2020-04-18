using System;
using System.Collections.Generic;

namespace ShoppingList.Database.Entities
{
    public partial class Store
    {
        public Store()
        {
            Item = new HashSet<Item>();
            ShoppingList = new HashSet<ShoppingList>();
        }

        public uint StoreId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Item> Item { get; set; }
        public virtual ICollection<ShoppingList> ShoppingList { get; set; }
    }
}
