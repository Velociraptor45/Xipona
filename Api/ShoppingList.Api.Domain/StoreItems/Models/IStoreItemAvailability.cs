namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public interface IStoreItemAvailability
    {
        StoreItemStoreId StoreId { get; }
        float Price { get; }
        IStoreItemSection DefaultSection { get; }
    }
}