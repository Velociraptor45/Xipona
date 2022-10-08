using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.SearchItemsByItemCategory;

public class SearchItemsByItemCategoryQuery : IQuery<IEnumerable<SearchItemByItemCategoryResult>>
{
    public SearchItemsByItemCategoryQuery(ItemCategoryId itemCategoryId)
    {
        ItemCategoryId = itemCategoryId;
    }

    public ItemCategoryId ItemCategoryId { get; }
}