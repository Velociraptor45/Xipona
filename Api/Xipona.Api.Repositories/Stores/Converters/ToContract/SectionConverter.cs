using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using Section = ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.Xipona.Api.Repositories.Stores.Converters.ToContract;

public class SectionConverter : IToContractConverter<(StoreId, ISection), Section>
{
    public Section ToContract((StoreId, ISection) source)
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