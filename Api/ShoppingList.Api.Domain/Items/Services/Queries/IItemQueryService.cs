using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

public interface IItemQueryService
{
    Task<StoreItemReadModel> GetAsync(ItemId itemId);
}