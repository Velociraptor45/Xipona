using ShoppingList.Domain.Models;

namespace ShoppingList.Infrastructure.Converters
{
    public static class ItemCategoryConverter
    {
        public static ItemCategory ToDomain(this Entities.ItemCategory entity)
        {
            return new ItemCategory(
                new ItemCategoryId(entity.Id),
                entity.Name,
                entity.Deleted);
        }

        public static Entities.ItemCategory ToEntities(this ItemCategory model)
        {
            return new Entities.ItemCategory()
            {
                Id = model.Id.Value,
                Name = model.Name,
                Deleted = model.IsDeleted
            };
        }
    }
}