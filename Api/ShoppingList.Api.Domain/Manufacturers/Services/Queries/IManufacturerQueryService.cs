using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Queries;

public interface IManufacturerQueryService
{
    Task<IEnumerable<ManufacturerReadModel>> GetAllActiveAsync();

    Task<IEnumerable<ManufacturerSearchReadModel>> SearchAsync(string searchInput);

    Task<IManufacturer> GetAsync(ManufacturerId manufacturerId);
}