using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Items.ToDomain;

public class SearchItemByItemCategoryResultConverter :
    IToDomainConverter<SearchItemByItemCategoryResultContract, SearchItemByItemCategoryResult>
{
    public SearchItemByItemCategoryResult ToDomain(SearchItemByItemCategoryResultContract source)
    {
        return new SearchItemByItemCategoryResult(
            source.ItemId,
            source.ItemTypeId,
            source.Name,
            source.Availabilities.Select(av =>
                new SearchItemByItemCategoryAvailability(av.StoreId, av.StoreName, av.Price)));
    }
}