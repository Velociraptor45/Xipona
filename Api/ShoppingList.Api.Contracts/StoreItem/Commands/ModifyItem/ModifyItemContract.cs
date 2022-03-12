using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ChangeItem
{
    public class ModifyItemContract
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int QuantityType { get; set; }
        public float QuantityInPacket { get; set; }
        public int QuantityTypeInPacket { get; set; }
        public Guid ItemCategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}