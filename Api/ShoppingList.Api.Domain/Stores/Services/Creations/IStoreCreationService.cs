using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;

public interface IStoreCreationService
{
    Task<IStore> CreateAsync(StoreCreation creation);
}