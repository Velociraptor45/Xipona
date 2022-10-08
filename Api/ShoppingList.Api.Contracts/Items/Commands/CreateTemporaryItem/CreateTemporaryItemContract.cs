using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.Shared;
using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemContract
    {
        public Guid ClientSideId { get; set; }
        public string Name { get; set; }
        public ItemAvailabilityContract Availability { get; set; }
    }
}