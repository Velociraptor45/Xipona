using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get
{
    public class ItemStoreContract
    {
        public ItemStoreContract(Guid id, string name, IEnumerable<ItemSectionContract> sections)
        {
            Id = id;
            Name = name;
            Sections = sections.ToList().AsReadOnly();
        }

        public Guid Id { get; }
        public string Name { get; }
        public IReadOnlyCollection<ItemSectionContract> Sections { get; }
    }
}