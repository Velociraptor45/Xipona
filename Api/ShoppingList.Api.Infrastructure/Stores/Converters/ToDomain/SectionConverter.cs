using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using Section = ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Section;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Converters.ToDomain;

public class SectionConverter : IToDomainConverter<Section, ISection>
{
    private readonly ISectionFactory _sectionFactory;

    public SectionConverter(ISectionFactory sectionFactory)
    {
        _sectionFactory = sectionFactory;
    }

    public ISection ToDomain(Section source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return _sectionFactory.Create(
            new SectionId(source.Id),
            new SectionName(source.Name),
            source.SortIndex,
            source.IsDefaultSection);
    }
}