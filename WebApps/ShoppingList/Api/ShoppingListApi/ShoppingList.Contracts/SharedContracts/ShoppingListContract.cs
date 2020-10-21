using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Contracts.SharedContracts
{
    public class ShoppingListContract
    {
        private readonly IEnumerable<ShoppingListItemContract> items;

        public ShoppingListContract(int id, StoreContract store, IEnumerable<ShoppingListItemContract> items,
            DateTime? completionDate)
        {
            Id = id;
            Store = store;
            this.items = items;
            CompletionDate = completionDate;
        }

        public int Id { get; }
        public StoreContract Store { get; }
        public IReadOnlyCollection<ShoppingListItemContract> Items { get => items.ToList().AsReadOnly(); }
        public DateTime? CompletionDate { get; }
    }
}