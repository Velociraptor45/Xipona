using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.ItemCategorySearch;

public class ItemCategorySearchQuery : IQuery<IEnumerable<ItemCategorySearchResultReadModel>>
{
    public ItemCategorySearchQuery(string searchInput, bool includeDeleted)
    {
        SearchInput = searchInput;
        IncludeDeleted = includeDeleted;
    }

    public string SearchInput { get; }
    public bool IncludeDeleted { get; }
}