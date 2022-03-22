using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.ItemSearchReadModels;

public interface IItemSearchReadModelConversionService
{
    Task<IEnumerable<SearchItemForShoppingResultReadModel>> ConvertAsync(IEnumerable<IStoreItem> items, IStore store,
        CancellationToken cancellationToken);

    Task<IEnumerable<SearchItemForShoppingResultReadModel>> ConvertAsync(IEnumerable<ItemWithMatchingItemTypeIds> itemTypes,
        IStore store, CancellationToken cancellationToken);
}