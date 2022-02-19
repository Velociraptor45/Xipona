using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.ItemSearchForShoppingLists;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.ItemSearchReadModels;

public interface IItemSearchReadModelConversionService
{
    Task<IEnumerable<ItemForShoppingListSearchReadModel>> ConvertAsync(IEnumerable<IStoreItem> items, IStore store,
        CancellationToken cancellationToken);

    Task<IEnumerable<ItemForShoppingListSearchReadModel>> ConvertAsync(IEnumerable<ItemWithMatchingItemTypeIds> itemTypes,
        IStore store, CancellationToken cancellationToken);
}