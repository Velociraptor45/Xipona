using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId
{
    public class ShoppingListContract
    {
        private readonly IEnumerable<ShoppingListSectionContract> _sections;

        public ShoppingListContract(Guid id, ShoppingListStoreContract store, IEnumerable<ShoppingListSectionContract> sections,
            DateTime? completionDate)
        {
            Id = id;
            Store = store;
            _sections = sections;
            CompletionDate = completionDate;
        }

        public Guid Id { get; }
        public ShoppingListStoreContract Store { get; }
        public IReadOnlyCollection<ShoppingListSectionContract> Sections { get => _sections.ToList().AsReadOnly(); }
        public DateTime? CompletionDate { get; }
    }
}