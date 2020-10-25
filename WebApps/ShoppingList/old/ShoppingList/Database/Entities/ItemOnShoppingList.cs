using System;
using System.Collections.Generic;

namespace ShoppingList.Database.Entities
{
    public partial class ItemOnShoppingList
    {
        public uint ItemOnShoppingListId { get; set; }
        public uint ShoppingListId { get; set; }
        public uint ItemId { get; set; }
        public uint? Quantity { get; set; }
        public bool IsInShoppingBasket { get; set; }

        public virtual Item Item { get; set; }
        public virtual ShoppingList ShoppingList { get; set; }
    }
}
