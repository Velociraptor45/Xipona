namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;

public interface IStoreCreationService
{
    Task CreateAsync(StoreCreation creation);
}