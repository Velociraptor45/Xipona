using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.Commands.UpdateItem
{
    public class ItemUpdate
    {
        private readonly IEnumerable<StoreItemAvailability> availabilities;

        public ItemUpdate(StoreItemId oldId, string name, string comment,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategoryId itemCategoryId, ManufacturerId manufacturerId,
            IEnumerable<StoreItemAvailability> availabilities)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
            }

            OldId = oldId ?? throw new System.ArgumentNullException(nameof(oldId));
            Name = name;
            Comment = comment;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategoryId = itemCategoryId ?? throw new System.ArgumentNullException(nameof(itemCategoryId));
            ManufacturerId = manufacturerId;
            this.availabilities = availabilities ?? throw new System.ArgumentNullException(nameof(availabilities));
        }

        public StoreItemId OldId { get; }
        public string Name { get; }
        public string Comment { get; }
        public QuantityType QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityTypeInPacket QuantityTypeInPacket { get; }
        public ItemCategoryId ItemCategoryId { get; }
        public ManufacturerId ManufacturerId { get; }

        public IReadOnlyCollection<StoreItemAvailability> Availabilities => availabilities.ToList().AsReadOnly();
    }
}