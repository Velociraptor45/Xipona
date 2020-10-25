using ShoppingList.Domain.Models;

namespace ShoppingList.Infrastructure.Converters
{
    public static class StoreConverter
    {
        public static Store ToDomain(this Entities.Store entity)
        {
            return new Store(
                new StoreId(entity.Id),
                entity.Name,
                entity.Deleted);
        }

        public static Entities.Store ToEntity(this Store model)
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