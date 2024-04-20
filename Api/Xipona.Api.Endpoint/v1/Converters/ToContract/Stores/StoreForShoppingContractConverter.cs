using ProjectHermes.Xipona.Api.Contracts.Stores.Queries.GetActiveStoresForShopping;
using ProjectHermes.Xipona.Api.Core.Converter;
using ProjectHermes.Xipona.Api.Domain.Stores.Models;

namespace ProjectHermes.Xipona.Api.Endpoint.v1.Converters.ToContract.Stores;

public class StoreForShoppingContractConverter : IToContractConverter<IStore, StoreForShoppingContract>
{
    private readonly IToContractConverter<ISection, SectionForShoppingContract> _sectionConverter;

    public StoreForShoppingContractConverter(
        IToContractConverter<ISection, SectionForShoppingContract> sectionConverter)
    {
        _sectionConverter = sectionConverter;
    }

    public StoreForShoppingContract ToContract(IStore source)
    {
        var sectionsToConvert = source.Sections.Where(s => !s.IsDeleted);
        return new StoreForShoppingContract(
            source.Id,
            source.Name,
            sectionsToConvert.Select(_sectionConverter.ToContract));
    }
}