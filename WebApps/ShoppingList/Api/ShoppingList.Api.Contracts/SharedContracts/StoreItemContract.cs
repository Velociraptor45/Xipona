using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Contracts.SharedContracts
{
    public class StoreItemContract
    {
        private readonly IEnumerable<StoreItemAvailabilityContract> availabilities;

        public StoreItemContract(int id, string name, bool isDeleted, string comment, bool isTemporary,
            int quantityType, float quantityInPacket, int quantityTypeInPacket,
            ItemCategoryContract itemCategory, ManufacturerContract manufacturer,
            IEnumerable<StoreItemAvailabilityContract> availabilities)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
            Comment = comment;
            IsTemporary = isTemporary;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategory = itemCategory ?? throw new System.ArgumentNullException(nameof(itemCategory));
            Manufacturer = manufacturer ?? throw new System.ArgumentNullException(nameof(manufacturer));
            this.availabilities = availabilities ?? throw new System.ArgumentNullException(nameof(availabilities));
        }

        public int Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public int QuantityType { get; }
        public float QuantityInPacket { get; }
        public int QuantityTypeInPacket { get; }
        public ItemCategoryContract ItemCategory { get; }
        public ManufacturerContract Manufacturer { get; }

        public IReadOnlyCollection<StoreItemAvailabilityContract> Availabilities => availabilities.ToList().AsReadOnly();
    }
}