using System;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.Shared
{
    /// <summary>
    /// Represents the search result for an item.
    /// </summary>
    public class SearchItemResultContract
    {
        /// <summary>
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemName"></param>
        /// <param name="manufacturerName"></param>
        public SearchItemResultContract(Guid itemId, string itemName, string manufacturerName)
        {
            ItemId = itemId;
            ItemName = itemName;
            ManufacturerName = manufacturerName;
        }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public Guid ItemId { get; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        public string ItemName { get; }

        /// <summary>
        /// The name of the item's manufacturer.
        /// </summary>
        public string ManufacturerName { get; }
    }
}