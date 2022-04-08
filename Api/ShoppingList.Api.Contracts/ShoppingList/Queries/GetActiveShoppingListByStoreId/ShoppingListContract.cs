using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId
{
    public class ShoppingListContract
    {
        public ShoppingListContract(Guid id, ShoppingListStoreContract store,
            IEnumerable<ShoppingListSectionContract> sections, DateTimeOffset? completionDate)
        {
            Id = id;
            Store = store;
            Sections = sections.ToList();
            CompletionDate = completionDate;
        }

        public Guid Id { get; }
        public ShoppingListStoreContract Store { get; }
        public IReadOnlyCollection<ShoppingListSectionContract> Sections { get; }
        public DateTimeOffset? CompletionDate { get; }
    }
}