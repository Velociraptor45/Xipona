using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Common.Models;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;
using Section = ProjectHermes.Xipona.Api.Repositories.Stores.Entities.Section;

namespace ProjectHermes.Xipona.Api.Repositories.Stores.Converters.ToContract;

public class StoreConverter : IToContractConverter<IStore, Entities.Store>
{
    private readonly IToContractConverter<(StoreId, ISection), Section> _sectionConverter;

    public StoreConverter(IToContractConverter<(StoreId, ISection), Section> sectionConverter)
    {
        _sectionConverter = sectionConverter;
    }

    public Entities.Store ToContract(IStore source)
    {
        return new Entities.Store()
        {
            Id = source.Id,
            Name = source.Name,
            Deleted = source.IsDeleted,
            Sections = source.Sections.Select(s => _sectionConverter.ToContract((source.Id, s))).ToList(),
            CreatedAt = source.CreatedAt,
            RowVersion = ((AggregateRoot)source).RowVersion
        };
    }
}