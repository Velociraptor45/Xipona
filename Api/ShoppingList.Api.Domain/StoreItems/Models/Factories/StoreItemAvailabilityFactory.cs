using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public class StoreItemAvailabilityFactory : IStoreItemAvailabilityFactory
    {
        public IStoreItemAvailability Create(StoreId storeId, float price, SectionId defaultSectionId)
        {
            return new StoreItemAvailability(storeId, price, defaultSectionId);
        }
    }
}