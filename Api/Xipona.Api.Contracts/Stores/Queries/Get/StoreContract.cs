using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Get
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