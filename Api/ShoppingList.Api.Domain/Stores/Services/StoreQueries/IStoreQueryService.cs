namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.StoreQueries;

public interface IStoreQueryService
{
    Task<IEnumerable<StoreReadModel>> GetActiveAsync();
}