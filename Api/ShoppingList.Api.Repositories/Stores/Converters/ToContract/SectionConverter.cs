using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using Section = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToContract;

public class SectionConverter : IToContractConverter<ISection, Entities.Section>
{
    public Section ToContract(ISection source)
    {
        return new Section
        {
            Id = source.Id,
            Name = source.Name,
            SortIndex = source.SortingIndex,
            IsDefaultSection = source.IsDefaultSection,
            IsDeleted = source.IsDeleted
        };
    }
}