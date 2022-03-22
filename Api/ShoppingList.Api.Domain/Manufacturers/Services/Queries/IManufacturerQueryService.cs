using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Queries;

public interface IManufacturerQueryService
{
    Task<IEnumerable<ManufacturerReadModel>> GetAllActive();

    Task<IEnumerable<ManufacturerReadModel>> Get(string searchInput);
}