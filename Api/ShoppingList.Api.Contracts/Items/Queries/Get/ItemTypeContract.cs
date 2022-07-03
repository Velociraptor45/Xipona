using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get
{
    public class ItemTypeContract
    {
        public ItemTypeContract(Guid id, string name, IEnumerable<ItemAvailabilityContract> availabilities)
        {
            Id = id;
            Name = name;
            Availabilities = availabilities;
        }

        public Guid Id { get; }
        public string Name { get; }
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; }
    }
}