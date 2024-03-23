using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using Section = ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.Xipona.Api.Repositories.Stores.Converters.ToEntity;

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
            CreatedAt = source.CreatedAt,
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }
}