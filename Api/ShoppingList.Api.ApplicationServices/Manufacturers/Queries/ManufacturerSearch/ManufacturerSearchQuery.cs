using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Queries.ManufacturerSearch;

public class ManufacturerSearchQuery : IQuery<IEnumerable<ManufacturerReadModel>>
{
    public ManufacturerSearchQuery(string searchInput)
    {
        SearchInput = searchInput;
    }

    public string SearchInput { get; }
}