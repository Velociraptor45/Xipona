using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get
{
    public class ItemTypeContract
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<StoreItemAvailabilityContract> Availabilities { get; set; }
    }
}