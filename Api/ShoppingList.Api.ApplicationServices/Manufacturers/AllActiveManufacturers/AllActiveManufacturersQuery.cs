using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.AllActiveManufacturers;

public class AllActiveManufacturersQuery : IQuery<IEnumerable<ManufacturerReadModel>>
{
}