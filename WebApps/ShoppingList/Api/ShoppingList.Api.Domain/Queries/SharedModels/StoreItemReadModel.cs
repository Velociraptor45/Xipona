using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.AllQuantityInPacketTypes;
using ShoppingList.Api.Domain.Queries.AllQuantityTypes;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.Queries.SharedModels
{
    public class StoreItemReadModel
    {
        private readonly IEnumerable<StoreItemAvailabilityReadModel> availabilities;

        public StoreItemReadModel(StoreItemId id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityTypeReadModel quantityType, float quantityInPacket, QuantityInPacketTypeReadModel quantityTypeInPacket,
            ItemCategoryReadModel itemCategory, ManufacturerReadModel manufacturer,
            IEnumerable<StoreItemAvailabilityReadModel> availabilities)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }

            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            QuantityType = quantityType ?? throw new System.ArgumentNullException(nameof(quantityType));
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket ?? throw new System.ArgumentNullException(nameof(quantityTypeInPacket));
            ItemCategory = itemCategory ?? throw new System.ArgumentNullException(nameof(itemCategory));
            Manufacturer = manufacturer ?? throw new System.ArgumentNullException(nameof(manufacturer));
            this.availabilities = availabilities ?? throw new System.ArgumentNullException(nameof(availabilities));
        }

        public StoreItemId Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public QuantityTypeReadModel QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityInPacketTypeReadModel QuantityTypeInPacket { get; }
        public ItemCategoryReadModel ItemCategory { get; }
        public ManufacturerReadModel Manufacturer { get; }

        public IReadOnlyCollection<StoreItemAvailabilityReadModel> Availabilities => availabilities.ToList().AsReadOnly();
    }
}