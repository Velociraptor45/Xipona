using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemContract
    {
        public Guid ClientSideId { get; set; }
        public string Name { get; set; }
        public ItemAvailabilityContract Availability { get; set; }
    }
}