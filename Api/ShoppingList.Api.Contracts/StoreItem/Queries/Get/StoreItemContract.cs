using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get
{
    public class StoreItemContract
    {
        private readonly IEnumerable<StoreItemAvailabilityContract> availabilities;

        public StoreItemContract(int id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityTypeContract quantityType, float quantityInPacket, QuantityInPacketTypeContract quantityTypeInPacket,
            ItemCategoryContract itemCategory, ManufacturerContract manufacturer,
            IEnumerable<StoreItemAvailabilityContract> availabilities)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentException($"'{nameof(name)}' cannot be null or empty", nameof(name));
            }

            Id = id;
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

        public int Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public QuantityTypeContract QuantityType { get; }
        public float QuantityInPacket { get; }
        public QuantityInPacketTypeContract QuantityTypeInPacket { get; }
        public ItemCategoryContract ItemCategory { get; }
        public ManufacturerContract Manufacturer { get; }

        public IReadOnlyCollection<StoreItemAvailabilityContract> Availabilities => availabilities.ToList().AsReadOnly();
    }
}