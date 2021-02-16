namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public interface IStoreItemAvailabilityFactory
    {
        IStoreItemAvailability Create(StoreItemStoreId id, float price, IStoreItemSection defaultSection);
    }
}