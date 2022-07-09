using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.AllActiveStores
{
    public class ActiveStoreContract
    {
        public ActiveStoreContract(Guid id, string name, int itemCount, IEnumerable<SectionContract> sections)
        {
            Id = id;
            Name = name;
            ItemCount = itemCount;
            Sections = sections.ToList().AsReadOnly();
        }

        public Guid Id { get; }
        public string Name { get; }
        public int ItemCount { get; }
        public IReadOnlyCollection<SectionContract> Sections { get; }
    }
}