using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public interface IStoreItemAvailability
    {
        StoreId StoreId { get; }
        float Price { get; }
    }
}