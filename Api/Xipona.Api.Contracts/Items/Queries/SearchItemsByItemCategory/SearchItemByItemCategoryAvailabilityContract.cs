using System;

namespace ProjectHermes.Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory
{
    public class SearchItemByItemCategoryAvailabilityContract
    {
        public SearchItemByItemCategoryAvailabilityContract(Guid storeId, string storeName, decimal price)
        {
            StoreId = storeId;
            StoreName = storeName;
            Price = price;
        }

        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        public decimal Price { get; set; }
    }
}