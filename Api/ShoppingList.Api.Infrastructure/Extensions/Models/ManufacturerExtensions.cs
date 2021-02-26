using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Models
{
    public static class ManufacturerExtensions
    {
        public static Infrastructure.Entities.Manufacturer ToEntity(this IManufacturer model)
        {
            return new Infrastructure.Entities.Manufacturer()
            {
                Id = model.Id.Value,
                Name = model.Name,
                Deleted = model.IsDeleted
            };
        }
    }
}