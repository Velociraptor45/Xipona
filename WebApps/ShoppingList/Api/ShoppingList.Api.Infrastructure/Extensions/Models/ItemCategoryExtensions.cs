namespace ShoppingList.Api.Infrastructure.Extensions.Models
{
    public static class ItemCategoryExtensions
    {
        public static Infrastructure.Entities.ItemCategory ToEntity(this Domain.Models.ItemCategory model)
        {
            return new Infrastructure.Entities.ItemCategory()
            {
                Id = model.Id.Value,
                Name = model.Name,
                Deleted = model.IsDeleted
            };
        }
    }
}