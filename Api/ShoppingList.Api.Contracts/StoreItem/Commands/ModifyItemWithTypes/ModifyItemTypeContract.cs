using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes
{
    public class ModifyItemTypeContract
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}