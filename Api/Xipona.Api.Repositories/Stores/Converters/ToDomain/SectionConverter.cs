using Xipona.Api.Core.Converter;
using Xipona.Api.Domain.Stores.Models;
using Xipona.Api.Domain.Stores.Models.Factories;
using Section = Xipona.Api.Repositories.Stores.Entities.Section;

namespace Xipona.Api.Repositories.Stores.Converters.ToDomain;

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