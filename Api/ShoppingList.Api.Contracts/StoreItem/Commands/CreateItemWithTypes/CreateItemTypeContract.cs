using System.Collections.Generic;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItemWithTypes
{
    public class CreateItemTypeContract
    {
        public string Name { get; set; }
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}