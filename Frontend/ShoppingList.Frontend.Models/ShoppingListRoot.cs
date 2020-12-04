using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class ShoppingListRoot
    {
        public ShoppingListRoot(int id, DateTime? completionDate, Store store, IEnumerable<ShoppingListItem> items)
        {
            Id = id;
            CompletionDate = completionDate;
            Store = store;
            Items = items.ToList();
        }

        public int Id { get; }
        public DateTime? CompletionDate { get; }
        public Store Store { get; }
        public List<ShoppingListItem> Items { get; }
        public bool AnyItemInBasket => Items.FirstOrDefault(item => item.IsInBasket) != null;

        public ShoppingListItem GetItemById(int id)
        {
            return Items.FirstOrDefault(item => item.Id.ActualId == id);
        }
    }
}