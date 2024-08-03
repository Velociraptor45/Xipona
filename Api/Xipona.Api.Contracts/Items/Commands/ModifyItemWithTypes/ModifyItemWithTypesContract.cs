using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Commands.ModifyItemWithTypes
{
    /// <summary>
    /// Represents a command to modify an item with types.
    /// </summary>
    public class ModifyItemWithTypesContract
    {
        /// <summary>
        /// </summary>
        /// <param name="name"></param>
        /// <param name="comment"></param>
        /// <param name="quantityType"></param>
        /// <param name="quantityInPacket"></param>
        /// <param name="quantityTypeInPacket"></param>
        /// <param name="itemCategoryId"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="itemTypes"></param>
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

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The comment added to the item.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// The quantity type of the item.
        /// </summary>
        public int QuantityType { get; set; }

        /// <summary>
        /// The in packet quantity of the item. Only valid to set if <see cref="QuantityType"/> is 0 (Unit).
        /// </summary>
        public float? QuantityInPacket { get; set; }

        /// <summary>
        /// The quantity type in packet of the item. Only valid to set if <see cref="QuantityType"/> is 0 (Unit).
        /// </summary>
        public int? QuantityTypeInPacket { get; set; }

        /// <summary>
        /// The ID of the item's item category.
        /// </summary>
        public Guid ItemCategoryId { get; set; }

        /// <summary>
        /// The ID of the item's manufacturer.
        /// </summary>
        public Guid? ManufacturerId { get; set; }

        /// <summary>
        /// All types of the item.
        /// </summary>
        public IEnumerable<ModifyItemTypeContract> ItemTypes { get; set; }
    }
}