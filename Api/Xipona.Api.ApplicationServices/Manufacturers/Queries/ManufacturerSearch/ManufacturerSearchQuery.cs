using ProjectHermes.Xipona.Api.ApplicationServices.Common.Queries;
using ProjectHermes.Xipona.Api.Domain.Manufacturers.Services.Queries;

namespace ProjectHermes.Xipona.Api.ApplicationServices.Manufacturers.Queries.ManufacturerSearch;

public class ManufacturerSearchQuery : IQuery<IEnumerable<ManufacturerSearchResultReadModel>>
{
    public ManufacturerSearchQuery(string searchInput, bool includeDeleted)
    {
        SearchInput = searchInput;
        IncludeDeleted = includeDeleted;
    }

    public string SearchInput { get; }
    public bool IncludeDeleted { get; }
}