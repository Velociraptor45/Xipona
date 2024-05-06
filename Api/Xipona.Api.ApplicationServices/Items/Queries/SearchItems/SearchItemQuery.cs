using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Items.Services.Searches;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Items.Queries.SearchItems;

public class SearchItemQuery : IQuery<SearchItemResultsReadModel>
{
    public SearchItemQuery(string searchInput, int page, int pageSize)
    {
        SearchInput = searchInput;
        Page = page;
        PageSize = pageSize;
    }

    public string SearchInput { get; }
    public int Page { get; }
    public int PageSize { get; }
}