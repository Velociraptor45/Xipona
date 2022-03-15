using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels;

public interface IStoreItemReadModelConversionService
{
    Task<StoreItemReadModel> ConvertAsync(IStoreItem item, CancellationToken cancellationToken);
}