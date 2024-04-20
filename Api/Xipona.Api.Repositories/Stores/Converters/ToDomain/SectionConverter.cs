using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models.Factories;
using Section = ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.Xipona.Api.Repositories.Stores.Converters.ToDomain;

public class SectionConverter : IToDomainConverter<Entities.Section, ISection>
{
    private readonly ISectionFactory _sectionFactory;

    public SectionConverter(ISectionFactory sectionFactory)
    {
        _sectionFactory = sectionFactory;
    }

    public ISection ToDomain(Section source)
    {
        return _sectionFactory.Create(
            new SectionId(source.Id),
            new SectionName(source.Name),
            source.SortIndex,
            source.IsDefaultSection,
            source.IsDeleted);
    }
}