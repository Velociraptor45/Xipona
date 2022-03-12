namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreCreations;

public interface IStoreCreationService
{
    Task CreateAsync(StoreCreation creation);
}