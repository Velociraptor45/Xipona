using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Queries;

public interface IManufacturerQueryService
{
    Task<IEnumerable<ManufacturerReadModel>> GetAllActiveAsync();

    Task<IEnumerable<ManufacturerSearchResultReadModel>> SearchAsync(string searchInput, bool includeDeleted);

    Task<IManufacturer> GetAsync(ManufacturerId manufacturerId);
}