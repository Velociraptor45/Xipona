using ShoppingList.Api.Domain.Models;
using System;

namespace ShoppingList.Api.Domain.Commands.CreateTemporaryItem
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