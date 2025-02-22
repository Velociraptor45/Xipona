using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory
{
    /// <summary>
    /// Represents a search result for an item or item type in a specific category.
    /// </summary>
    public class SearchItemByItemCategoryResultContract
    {
        /// <summary>
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemTypeId"></param>
        /// <param name="name"></param>
        /// <param name="manufacturerName"></param>
        /// <param name="availabilities"></param>
        public SearchItemByItemCategoryResultContract(Guid itemId, Guid? itemTypeId, string name, string manufacturerName,
            IEnumerable<SearchItemByItemCategoryAvailabilityContract> availabilities)
        {
            ItemId = itemId;
            ItemTypeId = itemTypeId;
            Name = name;
            ManufacturerName = manufacturerName;
            Availabilities = availabilities;
        }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// The ID of the item type. Null if the search result is a full item, not an item type.
        /// </summary>
        public Guid? ItemTypeId { get; set; }

        /// <summary>
        /// The name of the item or item type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The item's manufacturer name.
        /// </summary>
        public string ManufacturerName { get; }

        /// <summary>
        /// The availabilities of the item or item type.
        /// </summary>
        public IEnumerable<SearchItemByItemCategoryAvailabilityContract> Availabilities { get; set; }
    }
}