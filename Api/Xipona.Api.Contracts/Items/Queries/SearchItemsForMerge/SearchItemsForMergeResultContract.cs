using System;

namespace Xipona.Api.Contracts.Items.Queries.SearchItemsForMerge
{
    /// <summary>
    /// Represents a search result for an item selectable for merging
    /// </summary>
    public class SearchItemsForMergeResultContract
    {
        /// <summary> 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="name"></param>
        /// <param name="itemCategoryId"></param>
        /// <param name="manufacturerId"></param>
        /// <param name="quantityType"></param>
        /// <param name="quantity"></param>
        /// <param name="quantityTypeInPacket"></param>
        public SearchItemsForMergeResultContract(Guid itemId, string name, Guid itemCategoryId, Guid? manufacturerId,
            int quantityType, float? quantity, int? quantityTypeInPacket)
        {
            ItemId = itemId;
            Name = name;
            ItemCategoryId = itemCategoryId;
            ManufacturerId = manufacturerId;
            QuantityType = quantityType;
            Quantity = quantity;
            QuantityTypeInPacket = quantityTypeInPacket;
        }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The ID of the item's item category.
        /// </summary>
        public Guid ItemCategoryId { get; set; }

        /// <summary>
        /// The ID of the item's manufacturer. <code>null</code> if it has no manufacturer.
        /// </summary>
        public Guid? ManufacturerId { get; set; }

        /// <summary>
        /// The quantity type of the item.
        /// </summary>
        public int QuantityType { get; set; }

        /// <summary>
        /// The quantity of the item. <code>null</code> if it has no quantity.
        /// </summary>
        public float? Quantity { get; set; }

        /// <summary>
        /// The quantity type in package of the item. <code>null</code> if it has no quantity in packet type.
        /// </summary>
        public int? QuantityTypeInPacket { get; set; }
    }
}
