using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion.StoreItemReadModels;

public interface IStoreItemReadModelConversionService
{
    Task<StoreItemReadModel> ConvertAsync(IItem item, CancellationToken cancellationToken);
}