using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get
{
    public class StoreItemContract
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string Comment { get; set; }
        public bool IsTemporary { get; set; }
        public QuantityTypeContract QuantityType { get; set; }
        public float QuantityInPacket { get; set; }
        public QuantityTypeInPacketContract QuantityTypeInPacket { get; set; }
        public ItemCategoryContract ItemCategory { get; set; }
        public ManufacturerContract Manufacturer { get; set; }
        public IEnumerable<StoreItemAvailabilityContract> Availabilities { get; set; }
        public IEnumerable<ItemTypeContract> ItemTypes { get; set; }
    }
}