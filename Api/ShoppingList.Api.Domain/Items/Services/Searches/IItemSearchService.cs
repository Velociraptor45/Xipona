using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

public interface IItemSearchService
{
    Task<IEnumerable<SearchItemForShoppingResultReadModel>> SearchForShoppingListAsync(string name, StoreId storeId);

    Task<IEnumerable<SearchItemResultReadModel>> SearchAsync(string searchInput, ItemCategoryId? itemCategoryId);

    Task<IEnumerable<SearchItemResultReadModel>> SearchAsync(IEnumerable<StoreId> storeIds,
        IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds);

    Task<IEnumerable<SearchItemByItemCategoryResult>> SearchAsync(ItemCategoryId itemCategoryId);
}