using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.SharedModels
{
    public class ShoppingListReadModel
    {
        private readonly IEnumerable<ShoppingListSectionReadModel> sections;

        public ShoppingListReadModel(ShoppingListId id, DateTime? completionDate, StoreReadModel store,
            IEnumerable<ShoppingListSectionReadModel> sections)
        {
            Id = id;
            CompletionDate = completionDate;
            Store = store;
            this.sections = sections;
        }

        public ShoppingListId Id { get; }
        public DateTime? CompletionDate { get; }
        public StoreReadModel Store { get; }
        public IReadOnlyCollection<ShoppingListSectionReadModel> Sections { get => sections.ToList().AsReadOnly(); }
    }
}