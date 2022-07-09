using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Shared
{
    public class StoreContract
    {
        public StoreContract(Guid id, string name, IEnumerable<SectionContract> sections)
        {
            Id = id;
            Name = name;
            Sections = sections.ToList().AsReadOnly();
        }

        public Guid Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<SectionContract> Sections { get; }
    }
}