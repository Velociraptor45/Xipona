using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Manufacturers.Models;
using Xipona.Api.Domain.Stores.Models;

namespace Xipona.Api.Domain.Items.Services.Searches;

public interface IItemSearchService
{
    Task<IEnumerable<SearchItemForShoppingResultReadModel>> SearchForShoppingListAsync(string name, StoreId storeId);

    Task<IEnumerable<SearchItemResultReadModel>> SearchAsync(string searchInput, int page, int pageSize);

    Task<IEnumerable<SearchItemResultReadModel>> SearchAsync(IEnumerable<StoreId> storeIds,
        IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds);

    Task<IEnumerable<SearchItemByItemCategoryResult>> SearchAsync(ItemCategoryId itemCategoryId);
    Task<int> GetTotalSearchResultCountAsync(string searchInput);
}