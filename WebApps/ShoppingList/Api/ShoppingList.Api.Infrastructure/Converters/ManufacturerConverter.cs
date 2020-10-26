using Models = ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Infrastructure.Converters
{
    public static class ManufacturerConverter
    {
        public static Models.Manufacturer ToDomain(this Entities.Manufacturer entity)
        {
            return new Models.Manufacturer(
                new Models.ManufacturerId(entity.Id),
                entity.Name,
                entity.Deleted);
        }

        public static Entities.Manufacturer ToEntity(this Models.Manufacturer model)
        {
            return new Entities.Manufacturer()
            {
                Id = model.Id.Value,
                Name = model.Name,
                Deleted = model.IsDeleted
            };
        }
    }
}