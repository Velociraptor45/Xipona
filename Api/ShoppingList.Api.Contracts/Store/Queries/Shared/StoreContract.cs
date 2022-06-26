using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.Shared
{
    public class StoreContract
    {
        public StoreContract(Guid id, string name, IEnumerable<StoreSectionContract> sections)
        {
            Id = id;
            Name = name;
            Sections = sections.ToList().AsReadOnly();
        }

        public Guid Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<StoreSectionContract> Sections { get; }
    }
}