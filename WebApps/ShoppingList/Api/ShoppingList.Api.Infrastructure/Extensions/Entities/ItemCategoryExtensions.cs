namespace ShoppingList.Api.Infrastructure.Extensions.Entities
{
    public static class ItemCategoryExtensions
    {
        public static Domain.Models.ItemCategory ToDomain(this Infrastructure.Entities.ItemCategory entity)
        {
            return new Domain.Models.ItemCategory(
                new Domain.Models.ItemCategoryId(entity.Id),
                entity.Name,
                entity.Deleted);
        }
    }
}