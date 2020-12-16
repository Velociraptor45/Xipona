using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public class StoreItemAvailabilityFactory : IStoreItemAvailabilityFactory
    {
        public IStoreItemAvailability Create(StoreId id, float price)
        {
            return new StoreItemAvailability(id, price);
        }
    }
}