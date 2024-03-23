using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.SearchItemsByFilters;

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