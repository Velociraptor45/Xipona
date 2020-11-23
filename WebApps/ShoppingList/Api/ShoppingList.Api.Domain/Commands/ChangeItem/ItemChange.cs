using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.Commands.ChangeItem
{
    public class ItemChange
    {
        private readonly IEnumerable<StoreItemAvailability> availabilities;

        public ItemChange(StoreItemId id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategoryId itemCategoryId, ManufacturerId manufacturerId,
            IEnumerable<StoreItemAvailability> availabilities)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or whitespace", nameof(name));
            }

            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategoryId = itemCategoryId ?? throw new System.ArgumentNullException(nameof(itemCategoryId));
            ManufacturerId = manufacturerId ?? throw new System.ArgumentNullException(nameof(manufacturerId));
            this.availabilities = availabilities ?? throw new System.ArgumentNullException(nameof(availabilities));
        }

        public StoreItemId Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public QuantityType QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityTypeInPacket QuantityTypeInPacket { get; }
        public ItemCategoryId ItemCategoryId { get; }
        public ManufacturerId ManufacturerId { get; }

        public IReadOnlyCollection<StoreItemAvailability> Availabilities => availabilities.ToList().AsReadOnly();
    }
}