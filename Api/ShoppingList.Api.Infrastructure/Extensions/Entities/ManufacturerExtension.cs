using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class ManufacturerExtension
    {
        public static IManufacturer ToDomain(this Infrastructure.Entities.Manufacturer entity)
        {
            return new Manufacturer(
                new ManufacturerId(entity.Id),
                entity.Name,
                entity.Deleted);
        }
    }
}