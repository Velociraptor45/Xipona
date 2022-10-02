using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

public class SearchItemByItemCategoryAvailability
{
    public SearchItemByItemCategoryAvailability(Guid storeId, string storeName, float price)
    {
        StoreId = storeId;
        StoreName = storeName;
        Price = price;
    }

    public Guid StoreId { get; }
    public string StoreName { get; }
    public float Price { get; }
}