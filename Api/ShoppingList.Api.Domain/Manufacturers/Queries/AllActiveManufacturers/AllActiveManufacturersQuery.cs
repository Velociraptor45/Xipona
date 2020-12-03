using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.AllActiveManufacturers
{
    public class AllActiveManufacturersQuery : IQuery<IEnumerable<ManufacturerReadModel>>
    {
    }
}