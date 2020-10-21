using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Domain.Models
{
    public class ShoppingList
    {
        private readonly IEnumerable<ShoppingListItem> items;

        public ShoppingList(ShoppingListId id, Store store, IEnumerable<ShoppingListItem> items, DateTime? completionDate)
        {
            Id = id;
            Store = store;
            this.items = items;
            CompletionDate = completionDate;
        }

        public ShoppingListId Id { get; }
        public Store Store { get; }
        public IReadOnlyCollection<ShoppingListItem> Items { get => items.ToList().AsReadOnly(); }
        public DateTime? CompletionDate { get; }
    }
}