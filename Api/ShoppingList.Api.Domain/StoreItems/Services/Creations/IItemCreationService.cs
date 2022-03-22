using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;

public interface IItemCreationService
{
    Task CreateItemWithTypesAsync(IStoreItem item);

    Task CreateAsync(ItemCreation creation);

    Task CreateTemporaryAsync(TemporaryItemCreation creation);
}