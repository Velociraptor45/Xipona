using Xipona.Api.ApplicationServices.Common.Queries;
using Xipona.Api.Domain.Items.Services.Searches;

namespace Xipona.Api.ApplicationServices.Items.Queries.SearchItems;

public class SearchItemQuery : IQuery<IEnumerable<SearchItemResultReadModel>>
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