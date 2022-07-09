using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities;
using Section = ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Entities.Section;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.Stores.Converters.ToEntity;

public class StoreConverter : IToEntityConverter<IStore, Entities.Store>
{
    private readonly IToEntityConverter<ISection, Section> _sectionConverter;

    public StoreConverter(IToEntityConverter<ISection, Section> sectionConverter)
    {
        _sectionConverter = sectionConverter;
    }

    public Entities.Store ToEntity(IStore source)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        return new Entities.Store()
        {
            Id = source.Id.Value,
            Name = source.Name.Value,
            Deleted = source.IsDeleted,
            Sections = _sectionConverter.ToEntity(source.Sections).ToList()
        };
    }
}