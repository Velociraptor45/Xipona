using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get
{
    public class ItemTypeContract
    {
        public ItemTypeContract(Guid id, string name, IEnumerable<StoreItemAvailabilityContract> availabilities)
        {
            Id = id;
            Name = name;
            Availabilities = availabilities;
        }

        public Guid Id { get; }
        public string Name { get; }
        public IEnumerable<StoreItemAvailabilityContract> Availabilities { get; }
    }
}