using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId
{
    public class ShoppingListContract
    {
        private readonly IEnumerable<ShoppingListSectionContract> sections;

        public ShoppingListContract(int id, StoreContract store, IEnumerable<ShoppingListSectionContract> sections,
            DateTime? completionDate)
        {
            Id = id;
            Store = store;
            this.sections = sections;
            CompletionDate = completionDate;
        }

        public int Id { get; }
        public StoreContract Store { get; }
        public IReadOnlyCollection<ShoppingListSectionContract> Sections { get => sections.ToList().AsReadOnly(); }
        public DateTime? CompletionDate { get; }
    }
}