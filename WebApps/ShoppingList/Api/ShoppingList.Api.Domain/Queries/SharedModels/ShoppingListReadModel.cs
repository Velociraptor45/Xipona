using ShoppingList.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.Queries.SharedModels
{
    public class ShoppingListReadModel
    {
        private readonly IEnumerable<ShoppingListItemReadModel> items;

        public ShoppingListReadModel(ShoppingListId id, DateTime? completionDate, StoreReadModel store,
            IEnumerable<ShoppingListItemReadModel> items)
        {
            Id = id;
            CompletionDate = completionDate;
            Store = store;
            this.items = items;
        }

        public ShoppingListId Id { get; }
        public DateTime? CompletionDate { get; }
        public StoreReadModel Store { get; }
        public IReadOnlyCollection<ShoppingListItemReadModel> Items { get => items.ToList().AsReadOnly(); }
    }
}