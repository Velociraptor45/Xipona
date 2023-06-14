using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using Section = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToEntity;

public class SectionConverter : IToEntityConverter<(StoreId, ISection), Section>
{
    public Section ToEntity((StoreId, ISection) source)
    {
        var (storeId, section) = source;

        return new Section
        {
            Id = section.Id,
            Name = section.Name,
            SortIndex = section.SortingIndex,
            IsDefaultSection = section.IsDefaultSection,
            IsDeleted = section.IsDeleted,
            StoreId = storeId.Value,
        };
    }
}