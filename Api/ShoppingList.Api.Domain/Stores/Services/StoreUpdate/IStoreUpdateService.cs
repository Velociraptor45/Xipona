namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreUpdate;

public interface IStoreUpdateService
{
    Task UpdateAsync(StoreUpdate update);
}