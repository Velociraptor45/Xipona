using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.Stores.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.Stores;

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