using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public interface IStoreItemAvailabilityFactory
    {
        IStoreItemAvailability Create(StoreId id, float price);
    }
}