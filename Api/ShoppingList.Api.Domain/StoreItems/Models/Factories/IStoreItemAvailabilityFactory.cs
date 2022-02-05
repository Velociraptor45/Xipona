using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

public interface IStoreItemAvailabilityFactory
{
    IStoreItemAvailability Create(StoreId storeId, float price, SectionId defaultSectionId);
}