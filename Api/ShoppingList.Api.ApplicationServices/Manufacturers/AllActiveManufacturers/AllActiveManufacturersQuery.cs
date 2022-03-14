using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.AllActiveManufacturers;

public class AllActiveManufacturersQuery : IQuery<IEnumerable<ManufacturerReadModel>>
{
}