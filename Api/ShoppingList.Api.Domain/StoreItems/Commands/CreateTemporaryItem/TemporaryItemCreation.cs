using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem
{
    public class TemporaryItemCreation
    {
        public TemporaryItemCreation(Guid clientSideId, string name, StoreItemAvailability availability)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
            }

            ClientSideId = clientSideId;
            Name = name;
            Availability = availability ?? throw new ArgumentNullException(nameof(availability));
        }

        public Guid ClientSideId { get; }
        public string Name { get; }
        public StoreItemAvailability Availability { get; }
    }
}