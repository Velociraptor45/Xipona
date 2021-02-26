using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem
{
    public class TemporaryItemCreation
    {
        public TemporaryItemCreation(Guid clientSideId, string name, ShortAvailability availability)
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
        public ShortAvailability Availability { get; }
    }
}