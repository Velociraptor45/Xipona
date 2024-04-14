using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.SearchItems;

public class SearchItemQuery : IQuery<IEnumerable<SearchItemResultReadModel>>
{
    public SearchItemQuery(string searchInput)
    {
        SearchInput = searchInput;
    }

    public string SearchInput { get; }
}