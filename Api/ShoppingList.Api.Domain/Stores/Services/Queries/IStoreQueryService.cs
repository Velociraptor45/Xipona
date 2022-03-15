namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Queries;

public interface IStoreQueryService
{
    Task<IEnumerable<StoreReadModel>> GetActiveAsync();
}