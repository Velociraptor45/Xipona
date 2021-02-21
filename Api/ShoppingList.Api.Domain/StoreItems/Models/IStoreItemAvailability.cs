namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public interface IStoreItemAvailability
    {
        IStoreItemStore Store { get; }
        float Price { get; }
        IStoreItemSection DefaultSection { get; }
    }
}