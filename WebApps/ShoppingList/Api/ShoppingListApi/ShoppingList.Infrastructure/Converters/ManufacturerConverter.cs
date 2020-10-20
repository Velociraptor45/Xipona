using ShoppingList.Domain.Models;

namespace ShoppingList.Infrastructure.Converters
{
    public static class ManufacturerConverter
    {
        public static Manufacturer ToDomain(this Entities.Manufacturer entity)
        {
            return new Manufacturer(
                new ManufacturerId(entity.Id),
                entity.Name,
                entity.Deleted);
        }

        public static Entities.Manufacturer ToEntity(this Manufacturer model)
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