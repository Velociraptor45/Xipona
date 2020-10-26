using Models = ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Infrastructure.Converters
{
    public static class StoreConverter
    {
        public static Models.Store ToDomain(this Entities.Store entity)
        {
            return new Models.Store(
                new Models.StoreId(entity.Id),
                entity.Name,
                entity.Deleted);
        }

        public static Entities.Store ToEntity(this Models.Store model)
        {
            return new Entities.Store()
            {
                Id = model.Id.Value,
                Name = model.Name,
                Deleted = model.IsDeleted
            };
        }
    }
}