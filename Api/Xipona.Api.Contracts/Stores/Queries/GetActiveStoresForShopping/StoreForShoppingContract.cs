using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping
{
    public class StoreForShoppingContract
    {
        public StoreForShoppingContract(Guid id, string name, IEnumerable<SectionForShoppingContract> sections)
        {
            Id = id;
            Name = name;
            Sections = sections;
        }

        public Guid Id { get; }
        public string Name { get; }
        public IEnumerable<SectionForShoppingContract> Sections { get; }
    }
}