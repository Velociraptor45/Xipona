namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Updates;

public interface IStoreUpdateService
{
    Task UpdateAsync(StoreUpdate update);
}