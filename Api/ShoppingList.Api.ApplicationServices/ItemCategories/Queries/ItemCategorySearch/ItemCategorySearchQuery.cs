using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.ItemCategorySearch;

public class ItemCategorySearchQuery : IQuery<IEnumerable<ItemCategoryReadModel>>
{
    public ItemCategorySearchQuery(string searchInput)
    {
        SearchInput = searchInput;
    }

    public string SearchInput { get; }
}