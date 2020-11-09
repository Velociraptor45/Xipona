namespace ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class ManufacturerExtension
    {
        public static Domain.Models.Manufacturer ToDomain(this Infrastructure.Entities.Manufacturer entity)
        {
            return new Domain.Models.Manufacturer(
                new Domain.Models.ManufacturerId(entity.Id),
                entity.Name,
                entity.Deleted);
        }
    }
}