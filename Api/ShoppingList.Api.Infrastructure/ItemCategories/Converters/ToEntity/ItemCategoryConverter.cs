using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ItemCategories.Converters.ToEntity;

public class ItemCategoryConverter : IToEntityConverter<IItemCategory, Entities.ItemCategory>
{
    public Entities.ItemCategory ToEntity(IItemCategory source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new Entities.ItemCategory()
        {
            Id = source.Id.Value,
            Name = source.Name,
            Deleted = source.IsDeleted
        };
    }
}