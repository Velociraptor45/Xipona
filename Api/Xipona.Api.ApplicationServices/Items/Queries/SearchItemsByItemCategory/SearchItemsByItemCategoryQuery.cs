using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.ItemCategories.Models;
using Xipona.Api.Domain.Items.Services.Searches;

namespace Xipona.Api.ApplicationServices.Items.Queries.SearchItemsByItemCategory;

public class SearchItemsByItemCategoryQuery : IQuery<IEnumerable<SearchItemByItemCategoryResult>>
{
    public SearchItemsByItemCategoryQuery(ItemCategoryId itemCategoryId)
    {
        ItemCategoryId = itemCategoryId;
    }

    public ItemCategoryId ItemCategoryId { get; }
}