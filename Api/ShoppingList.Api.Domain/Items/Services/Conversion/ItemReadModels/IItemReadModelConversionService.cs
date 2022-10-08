using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion.ItemReadModels;

public interface IItemReadModelConversionService
{
    Task<ItemReadModel> ConvertAsync(IItem item, CancellationToken cancellationToken);
}