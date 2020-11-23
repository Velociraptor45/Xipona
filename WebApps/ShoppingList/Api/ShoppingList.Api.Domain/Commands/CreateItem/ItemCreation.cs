using ShoppingList.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.Commands.CreateItem
{
    public class ItemCreation
    {
        private readonly IEnumerable<StoreItemAvailability> availabilities;

        public ItemCreation(string name, string comment, bool isTemporary, QuantityType quantityType,
            float quantityInPacket, QuantityTypeInPacket quantityInPacketType, ItemCategoryId itemCategoryId,
            ManufacturerId manufacturerId, IEnumerable<StoreItemAvailability> availabilities,
            Guid? clientSideId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
            }

            Name = name;
            Comment = comment;
            IsTemporary = isTemporary;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityInPacketType = quantityInPacketType;
            ItemCategoryId = itemCategoryId ?? throw new ArgumentNullException(nameof(itemCategoryId));
            ManufacturerId = manufacturerId ?? throw new ArgumentNullException(nameof(manufacturerId));
            this.availabilities = availabilities ?? throw new ArgumentNullException(nameof(availabilities));
            ClientSideId = clientSideId ?? throw new ArgumentNullException(nameof(clientSideId));
        }

        public string Name { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public QuantityType QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityTypeInPacket QuantityInPacketType { get; }
        public ItemCategoryId ItemCategoryId { get; }
        public ManufacturerId ManufacturerId { get; }
        public Guid? ClientSideId { get; }

        public IReadOnlyCollection<StoreItemAvailability> Availabilities => availabilities.ToList().AsReadOnly();
    }
}