using System;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing
{
    /// <summary>
    /// Represents the availability of an ingredient's item in a store.
    /// </summary>
    public class ItemAmountForOneServingAvailabilityContract
    {
        /// <summary>
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="storeName"></param>
        /// <param name="price"></param>
        public ItemAmountForOneServingAvailabilityContract(Guid storeId, string storeName, decimal price)
        {
            StoreId = storeId;
            StoreName = storeName;
            Price = price;
        }

        /// <summary>
        /// The ID of the store.
        /// </summary>
        public Guid StoreId { get; }

        /// <summary>
        /// The name of the store.
        /// </summary>
        public string StoreName { get; }

        /// <summary>
        /// The price of the item in the store.
        /// </summary>
        public decimal Price { get; }
    }
}