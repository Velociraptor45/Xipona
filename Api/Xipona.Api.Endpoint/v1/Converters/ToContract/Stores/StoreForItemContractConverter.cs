using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForItem;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Stores;

public class StoreForItemContractConverter : IToContractConverter<IStore, StoreForItemContract>
{
    private readonly IToContractConverter<ISection, SectionForItemContract> _sectionConverter;

    public StoreForItemContractConverter(
        IToContractConverter<ISection, SectionForItemContract> sectionConverter)
    {
        _sectionConverter = sectionConverter;
    }

    public StoreForItemContract ToContract(IStore source)
    {
        var sectionsToConvert = source.Sections.Where(s => !s.IsDeleted);
        return new StoreForItemContract(
            source.Id,
            source.Name,
            sectionsToConvert.Select(_sectionConverter.ToContract));
    }
}