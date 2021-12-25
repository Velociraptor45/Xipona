using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.ItemUpdate
{
    public class ItemTypeUpdate
    {
        public ItemTypeUpdate(ItemTypeId oldId, string name, IEnumerable<IStoreItemAvailability> availabilities)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));

            OldId = oldId ?? throw new ArgumentNullException(nameof(oldId));
            Name = name;
            Availabilities = availabilities?.ToList() ?? throw new ArgumentNullException(nameof(availabilities));
        }

        public ItemTypeId OldId { get; }
        public string Name { get; }
        public IReadOnlyCollection<IStoreItemAvailability> Availabilities { get; }
    }
}