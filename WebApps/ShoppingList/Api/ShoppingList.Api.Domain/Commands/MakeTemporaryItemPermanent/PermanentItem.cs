using ShoppingList.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.Commands.MakeTemporaryItemPermanent
{
    public class PermanentItem
    {
        private readonly IEnumerable<StoreItemAvailability> availabilities;

        public PermanentItem(StoreItemId id, string name, string comment, QuantityType quantityType,
            float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket, ItemCategoryId itemCategoryId,
            ManufacturerId manufacturerId, IEnumerable<StoreItemAvailability> availabilities)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
            }

            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name;
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategoryId = itemCategoryId ?? throw new ArgumentNullException(nameof(itemCategoryId));
            ManufacturerId = manufacturerId;
            this.availabilities = availabilities ?? throw new ArgumentNullException(nameof(availabilities));
        }

        public IReadOnlyCollection<StoreItemAvailability> Availabilities => availabilities.ToList().AsReadOnly();

        public StoreItemId Id { get; }
        public string Name { get; }
        public string Comment { get; }
        public QuantityType QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityTypeInPacket QuantityTypeInPacket { get; }
        public ItemCategoryId ItemCategoryId { get; }
        public ManufacturerId ManufacturerId { get; }
    }
}