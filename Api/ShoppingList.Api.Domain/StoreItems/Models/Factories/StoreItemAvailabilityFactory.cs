namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public class StoreItemAvailabilityFactory : IStoreItemAvailabilityFactory
    {
        public IStoreItemAvailability Create(StoreItemStoreId id, float price, IStoreItemSection defaultSection)
        {
            return new StoreItemAvailability(id, price, defaultSection);
        }
    }
}