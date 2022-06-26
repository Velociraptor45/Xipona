using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;

public interface IItemCreationService
{
    Task<StoreItemReadModel> CreateItemWithTypesAsync(IStoreItem item);

    Task<StoreItemReadModel> CreateAsync(ItemCreation creation);

    Task<StoreItemReadModel> CreateTemporaryAsync(TemporaryItemCreation creation);
}