using ShoppingList.Domain.Models;
using ShoppingList.Domain.Queries.SharedModels;

namespace ShoppingList.Domain.Converters
{
    public static class ManufacturerReadModelConverter
    {
        public static ManufacturerReadModel ToReadModel(this Manufacturer model)
        {
            return new ManufacturerReadModel(model.Id, model.Name, model.IsDeleted);
        }
    }
}