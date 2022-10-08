using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public interface IItemAvailability
{
    StoreId StoreId { get; }
    Price Price { get; }
    SectionId DefaultSectionId { get; }
}