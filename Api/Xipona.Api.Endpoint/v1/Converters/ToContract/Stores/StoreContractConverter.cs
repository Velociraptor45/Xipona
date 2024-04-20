using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Stores;

public class StoreContractConverter : IToContractConverter<IStore, StoreContract>
{
    private readonly IToContractConverter<ISection, SectionContract> _sectionConverter;

    public StoreContractConverter(
        IToContractConverter<ISection, SectionContract> sectionConverter)
    {
        _sectionConverter = sectionConverter;
    }

    public StoreContract ToContract(IStore source)
    {
        var sectionsToConvert = source.Sections.Where(s => !s.IsDeleted);
        var sections = _sectionConverter.ToContract(sectionsToConvert);
        return new StoreContract(source.Id, source.Name, sections);
    }
}