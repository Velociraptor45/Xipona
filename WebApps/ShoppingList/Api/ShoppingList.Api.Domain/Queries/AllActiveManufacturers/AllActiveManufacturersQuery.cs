using ShoppingList.Api.Domain.Queries.SharedModels;
using System.Collections.Generic;

namespace ShoppingList.Api.Domain.Queries.AllActiveManufacturers
{
    public class AllActiveManufacturersQuery : IQuery<IEnumerable<ManufacturerReadModel>>
    {
    }
}