using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using Section = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToEntity;

public class SectionConverter : IToEntityConverter<ISection, Entities.Section>
{
    public Section ToEntity(ISection source)
    {
        return new Section
        {
            Id = source.Id.Value,
            Name = source.Name.Value,
            SortIndex = source.SortingIndex,
            IsDefaultSection = source.IsDefaultSection,
            IsDeleted = source.IsDeleted
        };
    }
}