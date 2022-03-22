using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes
{
    public class ModifyItemWithTypesContract
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int QuantityType { get; set; }
        public float? QuantityInPacket { get; set; }
        public int? QuantityTypeInPacket { get; set; }
        public Guid ItemCategoryId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public IEnumerable<ModifyItemTypeContract> ItemTypes { get; set; }
    }
}