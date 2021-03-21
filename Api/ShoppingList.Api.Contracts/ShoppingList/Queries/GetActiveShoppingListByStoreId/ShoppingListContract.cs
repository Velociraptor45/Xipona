using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId
{
    public class ShoppingListContract
    {
        private readonly IEnumerable<ShoppingListSectionContract> sections;

        public ShoppingListContract(int id, ShoppingListStoreContract store, IEnumerable<ShoppingListSectionContract> sections,
            DateTime? completionDate)
        {
            Id = id;
            Store = store;
            this.sections = sections;
            CompletionDate = completionDate;
        }

        public int Id { get; }
        public ShoppingListStoreContract Store { get; }
        public IReadOnlyCollection<ShoppingListSectionContract> Sections { get => sections.ToList().AsReadOnly(); }
        public DateTime? CompletionDate { get; }
    }
}