using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Domain.Extensions
{
    public static class ManufacturerExtensions
    {
        public static ManufacturerReadModel ToReadModel(this Manufacturer model)
        {
            return new ManufacturerReadModel(model.Id, model.Name, model.IsDeleted);
        }
    }
}