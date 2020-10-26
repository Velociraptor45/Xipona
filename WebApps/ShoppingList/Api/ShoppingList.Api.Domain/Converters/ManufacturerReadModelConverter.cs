using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Domain.Converters
{
    public static class ManufacturerReadModelConverter
    {
        public static ManufacturerReadModel ToReadModel(this Manufacturer model)
        {
            return new ManufacturerReadModel(model.Id, model.Name, model.IsDeleted);
        }
    }
}