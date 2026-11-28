using Xipona.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using Xipona.Frontend.Infrastructure.Converters.Common;
using Xipona.Frontend.Redux.Recipes.States;
using System.Linq;

namespace Xipona.Frontend.Infrastructure.Converters.Items.ToDomain;

public class SearchItemByItemCategoryResultConverter :
    IToDomainConverter<SearchItemByItemCategoryResultContract, SearchItemByItemCategoryResult>
{
    public SearchItemByItemCategoryResult ToDomain(SearchItemByItemCategoryResultContract source)
    {
        return new SearchItemByItemCategoryResult(
            source.ItemId,
            source.ItemTypeId,
            source.Name,
            source.ManufacturerName,
            source.Availabilities
                .Select(av => new SearchItemByItemCategoryAvailability(av.StoreId, av.StoreName, av.Price))
                .ToList());
    }
}