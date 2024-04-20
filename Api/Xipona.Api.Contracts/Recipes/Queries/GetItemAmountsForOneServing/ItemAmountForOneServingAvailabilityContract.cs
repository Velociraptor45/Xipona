using System;

namespace ProjectHermes.Xipona.Api.Contracts.Recipes.Queries.GetItemAmountsForOneServing
{
    public class ItemAmountForOneServingAvailabilityContract
    {
        public ItemAmountForOneServingAvailabilityContract(Guid storeId, string storeName, float price)
        {
            StoreId = storeId;
            StoreName = storeName;
            Price = price;
        }

        public Guid StoreId { get; }
        public string StoreName { get; }
        public float Price { get; }
    }
}