using ProjectHermes.Xipona.Api.Contracts.Common.Queries;
using ProjectHermes.Xipona.Api.Contracts.Items.Queries.AllQuantityTypes;
using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.Get
{
    public class ItemContract
    {
        public ItemContract(Guid id, string name, bool isDeleted, string comment, bool isTemporary,
            QuantityTypeContract quantityType, float? quantityInPacket,
            QuantityTypeInPacketContract quantityTypeInPacket, ItemCategoryContract itemCategory,
            ManufacturerContract manufacturer, IEnumerable<ItemAvailabilityContract> availabilities,
            IEnumerable<ItemTypeContract> itemTypes)
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
            Availabilities = availabilities;
            ItemTypes = itemTypes;
        }

        public Guid Id { get; }
        public string Name { get; }
        public bool IsDeleted { get; }
        public string Comment { get; }
        public bool IsTemporary { get; }
        public QuantityTypeContract QuantityType { get; }
        public float? QuantityInPacket { get; }
        public QuantityTypeInPacketContract QuantityTypeInPacket { get; }
        public ItemCategoryContract ItemCategory { get; }
        public ManufacturerContract Manufacturer { get; }
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; }
        public IEnumerable<ItemTypeContract> ItemTypes { get; }
    }
}