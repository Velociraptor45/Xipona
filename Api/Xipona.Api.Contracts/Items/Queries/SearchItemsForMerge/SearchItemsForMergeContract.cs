using System;
using System.Collections.Generic;

namespace Xipona.Api.Contracts.Items.Queries.SearchItemsForMerge
{
    /// <summary>
    /// Represents a request for getting items that can be merged with an item of the given values
    /// </summary>
    public class SearchItemsForMergeContract
    {
        /// <summary> 
        /// </summary>
        /// <param name="itemCategory"></param>
        /// <param name="manufacturer"></param>
        /// <param name="quantityType"></param>
        /// <param name="quantity"></param>
        /// <param name="quantityTypeInPacket"></param>
        /// <param name="excludedItemIds"></param>
        public SearchItemsForMergeContract(Guid itemCategory, Guid? manufacturer, int quantityType, float? quantity,
            int? quantityTypeInPacket, IEnumerable<Guid> excludedItemIds)
        {
            ItemCategory = itemCategory;
            Manufacturer = manufacturer;
            QuantityType = quantityType;
            Quantity = quantity;
            QuantityTypeInPacket = quantityTypeInPacket;
            ExcludedItemIds = excludedItemIds;
        }

        /// <summary>
        /// The item category the item must have.
        /// </summary>
        public Guid ItemCategory { get; set; }

        /// <summary>
        /// The manufacturer the item must have. <code>null</code> if it must not have a manufacturer.
        /// </summary>
        public Guid? Manufacturer { get; set; }

        /// <summary>
        /// The quantity type the item must have.
        /// </summary>
        public int QuantityType { get; set; }

        /// <summary>
        /// The quantity the item must have. <code>null</code> if it must not have a quantity.
        /// </summary>
        public float? Quantity { get; set; }

        /// <summary>
        /// The quantity type in packet the item must have. <code>null</code> if it must not have a quantity in packet type.
        /// </summary>
        public int? QuantityTypeInPacket { get; set; }

        /// <summary>
        /// The IDs of items that should not be included in the search results (because they e.g. are already selected).
        /// </summary>
        public IEnumerable<Guid> ExcludedItemIds { get; set; }
    }
}
