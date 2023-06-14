using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using Section = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.ShoppingList.Api.Repositories.Stores.Converters.ToEntity;

public class StoreConverter : IToEntityConverter<IStore, Entities.Store>
{
    private readonly IToEntityConverter<(StoreId, ISection), Section> _sectionConverter;

    public StoreConverter(IToEntityConverter<(StoreId, ISection), Section> sectionConverter)
    {
        _sectionConverter = sectionConverter;
    }

    public Entities.Store ToEntity(IStore source)
    {
        return new Entities.Store()
        {
            Id = source.Id,
            Name = source.Name,
            Deleted = source.IsDeleted,
            Sections = source.Sections.Select(s => _sectionConverter.ToEntity((source.Id, s))).ToList(),
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }
}