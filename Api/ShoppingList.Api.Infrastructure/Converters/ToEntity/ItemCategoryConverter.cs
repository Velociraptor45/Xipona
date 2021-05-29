using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Converters.ToEntity
{
    public class ItemCategoryConverter : IToEntityConverter<IItemCategory, ItemCategories.Entities.ItemCategory>
    {
        public ItemCategories.Entities.ItemCategory ToEntity(IItemCategory source)
        {
            if (source is null)
                throw new System.ArgumentNullException(nameof(source));

            return new ItemCategories.Entities.ItemCategory()
            {
                Id = source.Id.Value,
                Name = source.Name,
                Deleted = source.IsDeleted
            };
        }
    }
}