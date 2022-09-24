using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.SearchItems;

public class SearchItemQuery : IQuery<IEnumerable<SearchItemResultReadModel>>
{
    public SearchItemQuery(string searchInput, ItemCategoryId? itemCategoryId)
    {
        SearchInput = searchInput;
        ItemCategoryId = itemCategoryId;
    }

    public string SearchInput { get; }
    public ItemCategoryId? ItemCategoryId { get; }
}