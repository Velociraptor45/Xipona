using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Models;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.SearchItemsByItemCategory;

public class SearchItemsByItemCategoryQuery : IQuery<IEnumerable<SearchItemByItemCategoryResult>>
{
    public SearchItemsByItemCategoryQuery(ItemCategoryId itemCategoryId)
    {
        ItemCategoryId = itemCategoryId;
    }

    public ItemCategoryId ItemCategoryId { get; }
}