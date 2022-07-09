using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Conversion.ItemSearchReadModels;

public interface IItemSearchReadModelConversionService
{
    Task<IEnumerable<SearchItemForShoppingResultReadModel>> ConvertAsync(IEnumerable<IItem> items, IStore store,
        CancellationToken cancellationToken);

    Task<IEnumerable<SearchItemForShoppingResultReadModel>> ConvertAsync(IEnumerable<ItemWithMatchingItemTypeIds> itemTypes,
        IStore store, CancellationToken cancellationToken);
}