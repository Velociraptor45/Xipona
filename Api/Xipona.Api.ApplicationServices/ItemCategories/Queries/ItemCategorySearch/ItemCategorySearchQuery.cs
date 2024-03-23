using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.ItemCategories.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.ItemCategories.Queries.ItemCategorySearch;

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