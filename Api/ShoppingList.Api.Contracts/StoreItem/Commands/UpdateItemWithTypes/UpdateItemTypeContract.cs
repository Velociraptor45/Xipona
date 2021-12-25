using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes
{
    public class UpdateItemTypeContract
    {
        public int OldId { get; set; }
        public string Name { get; set; }
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}