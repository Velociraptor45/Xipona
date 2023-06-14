using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.GetActiveStoresForItem
{
    public class StoreForItemContract
    {
        public StoreForItemContract(Guid id, string name, IEnumerable<SectionForItemContract> sections)
        {
            Id = id;
            Name = name;
            Sections = sections;
        }

        public Guid Id { get; }
        public string Name { get; }
        public IEnumerable<SectionForItemContract> Sections { get; }
    }
}