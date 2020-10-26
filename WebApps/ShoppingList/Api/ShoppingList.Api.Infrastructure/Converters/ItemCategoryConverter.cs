using Models = ShoppingList.Api.Domain.Models;

namespace ShoppingList.Api.Infrastructure.Converters
{
    public static class ItemCategoryConverter
    {
        public static Models.ItemCategory ToDomain(this Entities.ItemCategory entity)
        {
            return new Models.ItemCategory(
                new Models.ItemCategoryId(entity.Id),
                entity.Name,
                entity.Deleted);
        }

        public static Entities.ItemCategory ToEntities(this Models.ItemCategory model)
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