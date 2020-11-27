using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.Models
{
    public class StoreItem
    {
        private readonly IEnumerable<StoreItemAvailability> availabilities;

        public StoreItem(StoreItemId id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategory itemCategory, Manufacturer manufacturer,
            IEnumerable<StoreItemAvailability> availabilities)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            this.availabilities = availabilities ?? throw new ArgumentNullException(nameof(availabilities));
        }

        public StoreItemId Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; private set; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public QuantityType QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityTypeInPacket QuantityTypeInPacket { get; }

        public ItemCategory ItemCategory { get; }
        public Manufacturer Manufacturer { get; }

        public IReadOnlyCollection<StoreItemAvailability> Availabilities => availabilities.ToList().AsReadOnly();

        public void Delete()
        {
            IsDeleted = true;
        }

        public bool IsAvailableInStore(StoreId storeId)
        {
            return Availabilities.FirstOrDefault(av => av.StoreId == storeId) != null;
        }
    }
}