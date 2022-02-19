using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Search;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Queries.SearchItemsByFilters;

public class SearchItemsByFilterQuery : IQuery<IEnumerable<SearchItemResultReadModel>>
{
    public SearchItemsByFilterQuery(IEnumerable<StoreId> storeIds,
        IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds)
    {
        StoreIds = storeIds;
        ItemCategoriesIds = itemCategoriesIds;
        ManufacturerIds = manufacturerIds;
    }

    public IEnumerable<StoreId> StoreIds { get; }
    public IEnumerable<ItemCategoryId> ItemCategoriesIds { get; }
    public IEnumerable<ManufacturerId> ManufacturerIds { get; }
}