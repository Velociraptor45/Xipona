using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.ManufacturerSearch;

public class ManufacturerSearchQuery : IQuery<IEnumerable<ManufacturerReadModel>>
{
    public ManufacturerSearchQuery(string searchInput)
    {
        SearchInput = searchInput;
    }

    public string SearchInput { get; }
}