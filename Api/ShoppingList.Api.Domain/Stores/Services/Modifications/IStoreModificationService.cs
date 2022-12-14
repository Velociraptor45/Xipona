namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Modifications;

public interface IStoreModificationService
{
    Task ModifyAsync(StoreModification update);
}