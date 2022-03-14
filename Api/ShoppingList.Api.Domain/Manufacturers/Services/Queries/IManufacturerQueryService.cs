using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Queries;

public interface IManufacturerQueryService
{
    Task<IEnumerable<ManufacturerReadModel>> GetAllActive();
}