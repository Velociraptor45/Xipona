using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Converters.ToEntity;

public class ItemCategoryConverter : IToEntityConverter<IItemCategory, Entities.ItemCategory>
{
    public Entities.ItemCategory ToEntity(IItemCategory source)
    {
        return new Entities.ItemCategory
        {
            Id = source.Id,
            Name = source.Name,
            Deleted = source.IsDeleted,
            CreatedAt = source.CreatedAt,
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }
}