using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes
{
    public class ModifyItemWithTypesContract
    {
        public ModifyItemWithTypesContract(string name, string comment, int quantityType, float? quantityInPacket,
            int? quantityTypeInPacket, Guid itemCategoryId, Guid? manufacturerId,
            IEnumerable<ModifyItemTypeContract> itemTypes)
        {
            Name = name;
            Comment = comment;
            QuantityType = quantityType;
            QuantityInPacket = quantityInPacket;
            QuantityTypeInPacket = quantityTypeInPacket;
            ItemCategoryId = itemCategoryId;
            ManufacturerId = manufacturerId;
            ItemTypes = itemTypes;
        }

        public string Name { get; }
        public string Comment { get; }
        public int QuantityType { get; }
        public float? QuantityInPacket { get; }
        public int? QuantityTypeInPacket { get; }
        public Guid ItemCategoryId { get; }
        public Guid? ManufacturerId { get; }
        public IEnumerable<ModifyItemTypeContract> ItemTypes { get; }
    }
}