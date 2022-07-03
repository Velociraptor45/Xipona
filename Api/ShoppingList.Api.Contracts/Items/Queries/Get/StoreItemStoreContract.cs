using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get
{
    public class StoreItemStoreContract
    {
        public StoreItemStoreContract(Guid id, string name, IEnumerable<StoreItemSectionContract> sections)
        {
            Id = id;
            Name = name;
            Sections = sections.ToList().AsReadOnly();
        }

        public Guid Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<StoreItemSectionContract> Sections { get; }
    }
}