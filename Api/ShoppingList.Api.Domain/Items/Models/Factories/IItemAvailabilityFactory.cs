using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models.Factories;

public interface IItemAvailabilityFactory
{
    IItemAvailability Create(StoreId storeId, Price price, SectionId defaultSectionId);
}