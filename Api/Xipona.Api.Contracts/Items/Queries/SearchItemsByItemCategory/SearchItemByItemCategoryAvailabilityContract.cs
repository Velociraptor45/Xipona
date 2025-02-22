using System;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory
{
    /// <summary>
    /// Represents availability of an item in a store.
    /// </summary>
    public class SearchItemByItemCategoryAvailabilityContract
    {
        /// <summary>
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="storeName"></param>
        /// <param name="price"></param>
        public SearchItemByItemCategoryAvailabilityContract(Guid storeId, string storeName, decimal price)
        {
            StoreId = storeId;
            StoreName = storeName;
            Price = price;
        }

        /// <summary>
        /// The ID of the store.
        /// </summary>
        public Guid StoreId { get; set; }

        /// <summary>
        /// The name of the store.
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// The item's price in the store.
        /// </summary>
        public decimal Price { get; set; }
    }
}