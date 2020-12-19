using ProjectHermes.ShoppingList.Api.Domain.Common.Models;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class ItemCategoryExtensions
    {
        public static IItemCategory ToDomain(this Infrastructure.Entities.ItemCategory entity)
        {
            return new ItemCategory(
                new ItemCategoryId(entity.Id),
                entity.Name,
                entity.Deleted);
        }
    }
}