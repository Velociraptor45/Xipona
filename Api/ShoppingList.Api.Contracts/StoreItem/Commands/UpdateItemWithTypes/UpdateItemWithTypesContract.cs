using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes
{
    public class UpdateItemWithTypesContract
    {
        public Guid OldId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int QuantityType { get; set; }
        public float QuantityInPacket { get; set; }
        public int QuantityTypeInPacket { get; set; }
        public int ItemCategoryId { get; set; }
        public int? ManufacturerId { get; set; }
        public IEnumerable<UpdateItemTypeContract> ItemTypes { get; set; }
    }
}