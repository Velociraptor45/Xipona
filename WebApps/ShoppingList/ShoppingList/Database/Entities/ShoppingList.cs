using System;
using System.Collections.Generic;

namespace ShoppingList.Database.Entities
{
    public partial class ShoppingList
    {
        public ShoppingList()
        {
            ItemOnShoppingList = new HashSet<ItemOnShoppingList>();
        }

        public uint ShoppingListId { get; set; }
        public uint StoreId { get; set; }
        public DateTime? CompletionDate { get; set; }

        public virtual Store Store { get; set; }
        public virtual ICollection<ItemOnShoppingList> ItemOnShoppingList { get; set; }
    }
}
