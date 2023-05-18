using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;

public class ItemAmountForOneServingAvailability
{
    public ItemAmountForOneServingAvailability(StoreId storeId, string storeName, Price price)
    {
        StoreId = storeId;
        StoreName = storeName;
        Price = price;
    }

    public StoreId StoreId { get; }
    public string StoreName { get; }
    public Price Price { get; }
}