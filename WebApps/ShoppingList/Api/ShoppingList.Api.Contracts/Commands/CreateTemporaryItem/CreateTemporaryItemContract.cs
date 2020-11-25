using ShoppingList.Api.Contracts.Commands.SharedContracts;
using System;

namespace ShoppingList.Api.Contracts.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemContract
    {
        public Guid ClientSideId { get; set; }
        public string Name { get; set; }
        public ItemAvailabilityContract Availability { get; set; }
    }
}