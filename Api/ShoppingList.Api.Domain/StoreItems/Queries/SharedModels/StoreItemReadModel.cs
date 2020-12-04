using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypesInPacket;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels
{
    public class StoreItemReadModel
    {
        private readonly IEnumerable<StoreItemAvailabilityReadModel> availabilities;

        public StoreItemReadModel(StoreItemActualId id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityTypeReadModel quantityType, float quantityInPacket, QuantityTypeInPacketReadModel quantityTypeInPacket,
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
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            this.availabilities = availabilities ?? throw new System.ArgumentNullException(nameof(availabilities));
        }

        public StoreItemActualId Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public QuantityTypeReadModel QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityTypeInPacketReadModel QuantityTypeInPacket { get; }
        public ItemCategoryReadModel ItemCategory { get; }
        public ManufacturerReadModel Manufacturer { get; }

        public IReadOnlyCollection<StoreItemAvailabilityReadModel> Availabilities => availabilities.ToList().AsReadOnly();
    }
}