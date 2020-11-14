using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.Queries.SharedModels
{
    public class StoreItemReadModel
    {
        private readonly IEnumerable<StoreItemAvailabilityReadModel> availabilities;

        public StoreItemReadModel(StoreItemId id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
            ItemCategoryReadModel itemCategory, ManufacturerReadModel manufacturer,
            IEnumerable<StoreItemAvailabilityReadModel> availabilities)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            this.availabilities = availabilities;
        }

        public StoreItemId Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public QuantityType QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityTypeInPacket QuantityTypeInPacket { get; }
        public ItemCategoryReadModel ItemCategory { get; }
        public ManufacturerReadModel Manufacturer { get; }

        public IReadOnlyCollection<StoreItemAvailabilityReadModel> Availabilities => availabilities.ToList().AsReadOnly();
    }
}