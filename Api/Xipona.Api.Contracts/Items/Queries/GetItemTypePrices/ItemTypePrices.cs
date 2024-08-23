using System;
using System.Collections.Generic;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.GetItemTypePrices
{
    /// <summary>
    /// Represents the prices of all of an item's types in a store.
    /// </summary>
    public class ItemTypePrices
    {
        /// <summary>
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="storeId"></param>
        /// <param name="prices"></param>
        public ItemTypePrices(Guid itemId, Guid storeId, IEnumerable<ItemTypePrice> prices)
        {
            ItemId = itemId;
            StoreId = storeId;
            Prices = prices;
        }

        /// <summary>
        /// The ID of the item.
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// The ID of the store for which the prices are defined.
        /// </summary>
        public Guid StoreId { get; set; }

        /// <summary>
        /// The prices of the item types in the store.
        /// </summary>
        public IEnumerable<ItemTypePrice> Prices { get; set; }
    }
}