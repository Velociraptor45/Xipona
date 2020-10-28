using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Frontend.Models
{
    public class ShoppingListRoot
    {
        private readonly IEnumerable<ShoppingListItem> items;

        public ShoppingListRoot(int id, DateTime? completionDate, Store store, IEnumerable<ShoppingListItem> items)
        {
            Id = id;
            CompletionDate = completionDate;
            Store = store;
            this.items = items;
        }

        public int Id { get; }
        public DateTime? CompletionDate { get; }
        public Store Store { get; }
        public IReadOnlyCollection<ShoppingListItem> Items => items.ToList().AsReadOnly();
        public bool AnyItemInBasket => Items.FirstOrDefault(item => item.IsInBasket) != null;
    }
}