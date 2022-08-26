using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.SearchItemsByItemCategory
{
    public class SearchItemByItemCategoryAvailabilityContract
    {
        public SearchItemByItemCategoryAvailabilityContract(Guid storeId, string storeName, float price)
        {
            StoreId = storeId;
            StoreName = storeName;
            Price = price;
        }

        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        public float Price { get; set; }
    }
}