using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get
{
    public class ItemTypeContract
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<StoreItemAvailabilityContract> Availabilities { get; set; }
    }
}